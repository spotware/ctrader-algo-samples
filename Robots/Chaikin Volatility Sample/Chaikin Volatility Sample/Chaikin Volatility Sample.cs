using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Chaikin Volatility indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChaikinVolatilitySample : Robot
    {
        private double _volumeInUnits;

        private ChaikinVolatility _chaikinVolatility;

        private SimpleMovingAverage _simpleMovingAverage;

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

            _chaikinVolatility = Indicators.ChaikinVolatility(14, 10, MovingAverageType.Simple);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 9);
        }

        protected override void OnBar()
        {
            if (_chaikinVolatility.Result.Last(1) > 0)
            {
                if (Bars.ClosePrices.Last(1) > _simpleMovingAverage.Result.Last(1) && Bars.ClosePrices.Last(2) < _simpleMovingAverage.Result.Last(2))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
                else if (Bars.ClosePrices.Last(1) < _simpleMovingAverage.Result.Last(1) && Bars.ClosePrices.Last(2) > _simpleMovingAverage.Result.Last(2))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else
            {
                ClosePositions();
            }
        }

        private void ClosePositions()
        {
            foreach (var position in BotPositions)
            {
                ClosePosition(position);
            }
        }
    }
}