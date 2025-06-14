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
    public class GoldAdvancedStrategy : Robot
    {
        private double _volumeInUnits;
        private Supertrend _supertrend;
        private MacdHistogram _macd;
        private RelativeStrengthIndex _rsi;

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

        [Parameter("Label", DefaultValue = "GoldAdvancedStrategy", Group = "Trade")]
        public string Label { get; set; }

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

        [Parameter("RSI Period", DefaultValue = 14, Group = "RSI", MinValue = 1)]
        public int RsiPeriod { get; set; }

        [Parameter("RSI Oversold", DefaultValue = 30, Group = "RSI", MinValue = 1, MaxValue = 50)]
        public int Oversold { get; set; }

        [Parameter("RSI Overbought", DefaultValue = 70, Group = "RSI", MinValue = 50, MaxValue = 100)]
        public int Overbought { get; set; }

        protected override void OnStart()
        {
            if (SymbolName != "XAUUSD")
                Print("Warning: This cBot is designed for XAUUSD. Current symbol is {0}.", SymbolName);

            if (!UseDynamicVolume)
                _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            _supertrend = Indicators.Supertrend(SupertrendPeriods, SupertrendMultiplier);
            _macd = Indicators.MacdHistogram(Bars.ClosePrices, MacdLongCycle, MacdShortCycle, MacdSignalPeriods);
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, RsiPeriod);
        }

        protected override void OnBarClosed()
        {
            if (Symbol.Spread / Symbol.PipSize > MaxSpreadInPips)
                return;

            var upTrend = _supertrend.UpTrend.Last(0) < Bars.LowPrices.Last(0) && _supertrend.DownTrend.Last(1) > Bars.HighPrices.Last(1);
            var downTrend = _supertrend.DownTrend.Last(0) > Bars.HighPrices.Last(0) && _supertrend.UpTrend.Last(1) < Bars.LowPrices.Last(1);

            var macdCrossUp = _macd.Histogram.Last(0) > 0 && _macd.Histogram.Last(1) <= 0;
            var macdCrossDown = _macd.Histogram.Last(0) < 0 && _macd.Histogram.Last(1) >= 0;

            var volume = GetTradeVolume();

            if (upTrend && macdCrossUp && _rsi.Result.LastValue < Oversold)
            {
                ClosePositions(TradeType.Sell);
                ExecuteMarketOrder(TradeType.Buy, SymbolName, volume, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (downTrend && macdCrossDown && _rsi.Result.LastValue > Overbought)
            {
                ClosePositions(TradeType.Buy);
                ExecuteMarketOrder(TradeType.Sell, SymbolName, volume, Label, StopLossInPips, TakeProfitInPips);
            }
        }

        protected override void OnTick()
        {
            if (TrailingStopInPips <= 0)
                return;

            foreach (var position in Positions.FindAll(Label))
            {
                double? newStop;
                if (position.TradeType == TradeType.Buy)
                    newStop = Symbol.Bid - TrailingStopInPips * Symbol.PipSize;
                else
                    newStop = Symbol.Ask + TrailingStopInPips * Symbol.PipSize;

                if (position.TradeType == TradeType.Buy && (position.StopLoss == null || newStop > position.StopLoss))
                    ModifyPosition(position, newStop, position.TakeProfit);
                else if (position.TradeType == TradeType.Sell && (position.StopLoss == null || newStop < position.StopLoss))
                    ModifyPosition(position, newStop, position.TakeProfit);
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
