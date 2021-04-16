using cAlgo.API;
using cAlgo.API.Indicators;
using System;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Standard Deviation indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class StandardDeviationSample : Robot
    {
        private double _volumeInUnits;

        private StandardDeviation _standardDeviation;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

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

            _standardDeviation = Indicators.StandardDeviation(Bars.ClosePrices, 20, MovingAverageType.Simple);
            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
        }

        protected override void OnBar()
        {
            if (Bars.ClosePrices.HasCrossedAbove(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);

                ExecuteOrder(TradeType.Buy);
            }
            else if (Bars.ClosePrices.HasCrossedBelow(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Buy);

                ExecuteOrder(TradeType.Sell);
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

        private void ExecuteOrder(TradeType tradeType)
        {
            var standardDeviationInPips = _standardDeviation.Result.Last(1) * (Symbol.TickSize / Symbol.PipSize * Math.Pow(10, Symbol.Digits));

            var stopLossInPips = standardDeviationInPips * 2;
            var takeProfitInPips = stopLossInPips * 2;

            ExecuteMarketOrder(tradeType, SymbolName, _volumeInUnits, Label, stopLossInPips, takeProfitInPips);
        }
    }
}