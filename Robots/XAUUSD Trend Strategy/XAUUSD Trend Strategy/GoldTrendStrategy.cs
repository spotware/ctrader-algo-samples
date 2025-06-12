// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class GoldTrendStrategy : Robot
    {
        private double _volumeInUnits;
        private SimpleMovingAverage _fastMa;
        private SimpleMovingAverage _slowMa;
        private RelativeStrengthIndex _rsi;
        private Supertrend _supertrend;
        private MacdCrossOver _macd;

        [Parameter("Fast MA Source", Group = "Fast MA")]
        public DataSeries FastMaSource { get; set; }

        [Parameter("Fast MA Period", DefaultValue = 50, Group = "Fast MA", MinValue = 1)]
        public int FastMaPeriod { get; set; }

        [Parameter("Slow MA Source", Group = "Slow MA")]
        public DataSeries SlowMaSource { get; set; }

        [Parameter("Slow MA Period", DefaultValue = 200, Group = "Slow MA", MinValue = 1)]
        public int SlowMaPeriod { get; set; }

        [Parameter("RSI Period", DefaultValue = 14, Group = "RSI", MinValue = 1)]
        public int RsiPeriod { get; set; }

        [Parameter("RSI Oversold", DefaultValue = 30, Group = "RSI", MinValue = 1, MaxValue = 50)]
        public int Oversold { get; set; }

        [Parameter("RSI Overbought", DefaultValue = 70, Group = "RSI", MinValue = 50, MaxValue = 100)]
        public int Overbought { get; set; }

        [Parameter("Supertrend Periods", DefaultValue = 10, Group = "Supertrend", MinValue = 1)]
        public int SupertrendPeriods { get; set; }

        [Parameter("Supertrend Multiplier", DefaultValue = 3.0, Group = "Supertrend", MinValue = 0.1)]
        public double SupertrendMultiplier { get; set; }

        [Parameter("MACD Long Cycle", DefaultValue = 26, Group = "MACD", MinValue = 1)]
        public int MacdLongCycle { get; set; }

        [Parameter("MACD Short Cycle", DefaultValue = 12, Group = "MACD", MinValue = 1)]
        public int MacdShortCycle { get; set; }

        [Parameter("MACD Signal Periods", DefaultValue = 9, Group = "MACD", MinValue = 1)]
        public int MacdSignalPeriods { get; set; }

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }

        [Parameter("Use Dynamic Volume", DefaultValue = false, Group = "Trade")]
        public bool UseDynamicVolume { get; set; }

        [Parameter("Risk Per Trade (%)", DefaultValue = 1.0, Group = "Trade", MinValue = 0.1)]
        public double RiskPercent { get; set; }

        [Parameter("Max Spread (pips)", DefaultValue = 50, Group = "Trade", MinValue = 0)]
        public double MaxSpreadInPips { get; set; }

        [Parameter("Trailing Stop (Pips)", DefaultValue = 50, Group = "Trade", MinValue = 0)]
        public double TrailingStopInPips { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 100, Group = "Trade", MinValue = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 200, Group = "Trade", MinValue = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "GoldTrendStrategy", Group = "Trade")]
        public string Label { get; set; }

        protected override void OnStart()
        {
            if (SymbolName != "XAUUSD")
                Print("Warning: This cBot is designed for XAUUSD. Current symbol is {0}.", SymbolName);

            if (!UseDynamicVolume)
                _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);
            _fastMa = Indicators.SimpleMovingAverage(FastMaSource, FastMaPeriod);
            _slowMa = Indicators.SimpleMovingAverage(SlowMaSource, SlowMaPeriod);
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, RsiPeriod);
            _supertrend = Indicators.Supertrend(SupertrendPeriods, SupertrendMultiplier);
            _macd = Indicators.MacdCrossOver(Bars.ClosePrices, MacdLongCycle, MacdShortCycle, MacdSignalPeriods);

            _fastMa.Result.Line.Color = Color.Gold;
            _slowMa.Result.Line.Color = Color.DarkOrange;
        }

        protected override void OnBarClosed()
        {
            var trendUp = _supertrend.UpTrend.Last(0) < Bars.LowPrices.Last(0) && _supertrend.DownTrend.Last(1) > Bars.HighPrices.Last(1);
            var trendDown = _supertrend.DownTrend.Last(0) > Bars.HighPrices.Last(0) && _supertrend.UpTrend.Last(1) < Bars.LowPrices.Last(1);

            var macdCrossUp = _macd.MACD.Last(0) > _macd.Signal.Last(0) && _macd.MACD.Last(1) <= _macd.Signal.Last(1);
            var macdCrossDown = _macd.MACD.Last(0) < _macd.Signal.Last(0) && _macd.MACD.Last(1) >= _macd.Signal.Last(1);

            if (Symbol.Spread / Symbol.PipSize > MaxSpreadInPips)
                return;

            var volume = GetTradeVolume();

            if (trendUp && macdCrossUp && _rsi.Result.LastValue < Oversold)
            {
                ClosePositions(TradeType.Sell);
                ExecuteMarketOrder(TradeType.Buy, SymbolName, volume, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (trendDown && macdCrossDown && _rsi.Result.LastValue > Overbought)
            {
                ClosePositions(TradeType.Buy);
                ExecuteMarketOrder(TradeType.Sell, SymbolName, volume, Label, StopLossInPips, TakeProfitInPips);
            }
        }

        protected override void OnTick()
        {
            foreach (var position in Positions.FindAll(Label))
            {
                double? newStopLoss;

                if (position.TradeType == TradeType.Buy)
                    newStopLoss = Symbol.Bid - TrailingStopInPips * Symbol.PipSize;
                else
                    newStopLoss = Symbol.Ask + TrailingStopInPips * Symbol.PipSize;

                if (position.TradeType == TradeType.Buy && (position.StopLoss == null || newStopLoss > position.StopLoss))
                    ModifyPosition(position, newStopLoss, position.TakeProfit);
                else if (position.TradeType == TradeType.Sell && (position.StopLoss == null || newStopLoss < position.StopLoss))
                    ModifyPosition(position, newStopLoss, position.TakeProfit);
            }
        }

        private double GetTradeVolume()
        {
            if (!UseDynamicVolume)
                return _volumeInUnits;

            var riskAmount = Account.Balance * RiskPercent / 100.0;
            var volumeInLots = riskAmount / (StopLossInPips * Symbol.PipValue);
            var units = Symbol.QuantityToVolumeInUnits(volumeInLots);
            units = Math.Max(Symbol.VolumeInUnitsMin, Math.Min(Symbol.VolumeInUnitsMax, units));
            return Symbol.NormalizeVolumeInUnits(units, RoundingMode.ToNearest);
        }

        private void ClosePositions(TradeType tradeType)
        {
            foreach (var position in Positions.FindAll(Label))
            {
                if (position.TradeType != tradeType)
                    continue;

                ClosePosition(position);
            }
        }
    }
}
