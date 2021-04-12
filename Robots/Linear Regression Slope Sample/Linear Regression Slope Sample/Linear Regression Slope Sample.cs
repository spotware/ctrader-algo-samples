using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Linear Regression Slope indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class LinearRegressionSlopeSample : Robot
    {
        private double _volumeInUnits;

        private LinearRegressionSlope _linearRegressionSlope;

        private SimpleMovingAverage _simpleMovingAverage;

        private ExponentialMovingAverage _exponentialMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "Sample")]
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

            _linearRegressionSlope = Indicators.LinearRegressionSlope(Bars.ClosePrices, 20);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(_linearRegressionSlope.Result, 10);

            _exponentialMovingAverage = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 20);
        }

        protected override void OnBar()
        {
            if (Bars.ClosePrices.Last(1) > _exponentialMovingAverage.Result.Last(1) && Bars.ClosePrices.Last(2) <= _exponentialMovingAverage.Result.Last(2))
            {
                ClosePositions(TradeType.Sell);

                if (_linearRegressionSlope.Result.Last(1) > _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (Bars.ClosePrices.Last(1) < _exponentialMovingAverage.Result.Last(1) && Bars.ClosePrices.Last(2) >= _exponentialMovingAverage.Result.Last(2))
            {
                ClosePositions(TradeType.Buy);

                if (_linearRegressionSlope.Result.Last(1) > _simpleMovingAverage.Result.Last(1))
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