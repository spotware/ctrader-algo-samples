using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Exponential Moving Average indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ExponentialMovingAverageSample : Robot
    {
        private double _volumeInUnits;

        private ExponentialMovingAverage _fastExponentialMovingAverage;

        private ExponentialMovingAverage _slowExponentialMovingAverage;

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

            _fastExponentialMovingAverage = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 9);

            _slowExponentialMovingAverage = Indicators.ExponentialMovingAverage(Bars.ClosePrices, 20);
        }

        protected override void OnBar()
        {
            if (_fastExponentialMovingAverage.Result.Last(1) > _slowExponentialMovingAverage.Result.Last(1) && _fastExponentialMovingAverage.Result.Last(2) < _slowExponentialMovingAverage.Result.Last(2))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_fastExponentialMovingAverage.Result.Last(1) < _slowExponentialMovingAverage.Result.Last(1) && _fastExponentialMovingAverage.Result.Last(2) > _slowExponentialMovingAverage.Result.Last(2))
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