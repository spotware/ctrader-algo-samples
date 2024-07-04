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
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class LinearRegressionSlopeSample : Robot
    {
        private double _volumeInUnits;

        private LinearRegressionSlope _linearRegressionSlope;

        private SimpleMovingAverage _simpleMovingAverage;

        private ExponentialMovingAverage _exponentialMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "LinearRegressionSlopeSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Linear Regression")]
        public DataSeries SourceLinearRegression { get; set; }

        [Parameter("Periods", DefaultValue = 20, Group = "Linear Regression", MinValue = 0)]
        public int PeriodsLinearRegression { get; set; }

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceSimpleMovingAverage { get; set; }

        [Parameter("Periods", DefaultValue = 10, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }

        [Parameter("Source", Group = "Exponential Moving Average")]
        public DataSeries SourceExponentialMovingAverage { get; set; }

        [Parameter("Periods", DefaultValue = 20, Group = "Exponential Moving Average", MinValue = 0)]
        public int PeriodsExponentialMovingAverage { get; set; }

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

            _linearRegressionSlope = Indicators.LinearRegressionSlope(SourceLinearRegression, PeriodsLinearRegression);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(_linearRegressionSlope.Result, PeriodsSimpleMovingAverage);

            _exponentialMovingAverage = Indicators.ExponentialMovingAverage(SourceExponentialMovingAverage, PeriodsExponentialMovingAverage);
        }

        protected override void OnBarClosed()
        {
            if (Bars.ClosePrices.Last(0) > _exponentialMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) <= _exponentialMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);

                if (_linearRegressionSlope.Result.Last(0) > _simpleMovingAverage.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (Bars.ClosePrices.Last(0) < _exponentialMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) >= _exponentialMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Buy);

                if (_linearRegressionSlope.Result.Last(0) > _simpleMovingAverage.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
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