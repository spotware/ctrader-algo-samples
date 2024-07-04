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
    public class MovingAverageSample : Robot
    {
        private double _volumeInUnits;

        private MovingAverage _fastMa;

        private MovingAverage _slowMa;

        [Parameter("Source", Group = "Fast MA")]
        public DataSeries FastMaSource { get; set; }

        [Parameter("Period", DefaultValue = 9, Group = "Fast MA")]
        public int FastMaPeriod { get; set; }

        [Parameter("Type", DefaultValue = MovingAverageType.Exponential, Group = "Fast MA")]
        public MovingAverageType FastMaType { get; set; }

        [Parameter("Source", Group = "Slow MA")]
        public DataSeries SlowMaSource { get; set; }

        [Parameter("Period", DefaultValue = 20, Group = "Slow MA")]
        public int SlowMaPeriod { get; set; }

        [Parameter("Type", DefaultValue = MovingAverageType.Exponential, Group = "Slow MA")]
        public MovingAverageType SlowMaType { get; set; }

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "MovingAverageSample", Group = "Trade")]
        public string Label { get; set; }

        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);
            }
        }

        protected override void OnStart()
        {
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            _fastMa = Indicators.MovingAverage(FastMaSource, FastMaPeriod, FastMaType);
            _slowMa = Indicators.MovingAverage(SlowMaSource, SlowMaPeriod, SlowMaType);

            _fastMa.Result.Line.Color = Color.Blue;
            _slowMa.Result.Line.Color = Color.Red;
        }

        protected override void OnBarClosed()
        {
            if (_fastMa.Result.HasCrossedAbove(_slowMa.Result, 0))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_fastMa.Result.HasCrossedBelow(_slowMa.Result, 0))
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
        }

        private void ClosePositions(TradeType tradeType)
        {
            foreach (var position in BotPositions)
            {
                if (position.TradeType != tradeType) continue;

                ClosePosition(position);
            }
        }
    }
}