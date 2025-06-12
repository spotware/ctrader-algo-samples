// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class GoldTrendStrategy : Robot
    {
        private double _volumeInUnits;
        private SimpleMovingAverage _fastMa;
        private SimpleMovingAverage _slowMa;
        private RelativeStrengthIndex _rsi;

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

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }

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

            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);
            _fastMa = Indicators.SimpleMovingAverage(FastMaSource, FastMaPeriod);
            _slowMa = Indicators.SimpleMovingAverage(SlowMaSource, SlowMaPeriod);
            _rsi = Indicators.RelativeStrengthIndex(MarketSeries.Close, RsiPeriod);

            _fastMa.Result.Line.Color = Color.Gold;
            _slowMa.Result.Line.Color = Color.DarkOrange;
        }

        protected override void OnBarClosed()
        {
            var inUptrend = _fastMa.Result.LastValue > _slowMa.Result.LastValue;
            var inDowntrend = _fastMa.Result.LastValue < _slowMa.Result.LastValue;

            if (inUptrend && _rsi.Result.LastValue < Oversold)
            {
                ClosePositions(TradeType.Sell);
                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (inDowntrend && _rsi.Result.LastValue > Overbought)
            {
                ClosePositions(TradeType.Buy);
                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
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
