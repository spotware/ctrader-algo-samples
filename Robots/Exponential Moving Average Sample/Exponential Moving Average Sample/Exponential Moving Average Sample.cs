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
    public class ExponentialMovingAverageSample : Robot
    {
        private double _volumeInUnits;

        private ExponentialMovingAverage _fastExponentialMovingAverage;

        private ExponentialMovingAverage _slowExponentialMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "ExponentialMovingAverageSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 9, Group = "Exponential Moving Average 1", MinValue = 0)]
        public int PeriodsFirst { get; set; }

        [Parameter("Source", Group = "Exponential Moving Average 1")]
        public DataSeries SourceFirst { get; set; }


        [Parameter("Periods", DefaultValue = 20, Group = "Exponential Moving Average 2", MinValue = 0)]
        public int PeriodsSecond { get; set; }

        [Parameter("Source", Group = "Exponential Moving Average 2")]
        public DataSeries SourceSecond { get; set; }

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

            _fastExponentialMovingAverage = Indicators.ExponentialMovingAverage(SourceFirst, PeriodsFirst);

            _fastExponentialMovingAverage.Result.Line.Color = Color.Blue;

            _slowExponentialMovingAverage = Indicators.ExponentialMovingAverage(SourceSecond, PeriodsSecond);

            _slowExponentialMovingAverage.Result.Line.Color = Color.Red;
        }

        protected override void OnBarClosed()
        {
            if (_fastExponentialMovingAverage.Result.Last(0) > _slowExponentialMovingAverage.Result.Last(0) && _fastExponentialMovingAverage.Result.Last(1) < _slowExponentialMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_fastExponentialMovingAverage.Result.Last(0) < _slowExponentialMovingAverage.Result.Last(0) && _fastExponentialMovingAverage.Result.Last(1) > _slowExponentialMovingAverage.Result.Last(1))
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