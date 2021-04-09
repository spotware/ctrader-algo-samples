using cAlgo.API;
using cAlgo.API.Indicators;
using System;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use an Average True Range indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class AverageTrueRangeSample : Robot
    {
        private double _volumeInUnits;

        private AverageTrueRange _averageTrueRange;

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

            _averageTrueRange = Indicators.AverageTrueRange(14, MovingAverageType.Exponential);
        }

        protected override void OnBar()
        {
            if (Bars.ClosePrices.Last(1) > Bars.OpenPrices.Last(1) && Bars.ClosePrices.Last(2) < Bars.OpenPrices.Last(2))
            {
                ClosePositions(TradeType.Sell);

                ExecuteOrder(TradeType.Buy);
            }
            else if (Bars.ClosePrices.Last(1) < Bars.OpenPrices.Last(1) && Bars.ClosePrices.Last(2) > Bars.OpenPrices.Last(2))
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
            var atrInPips = _averageTrueRange.Result.Last(1) * (Symbol.TickSize / Symbol.PipSize * Math.Pow(10, Symbol.Digits));

            var stopLossInPips = atrInPips * 2;
            var takeProfitInPips = stopLossInPips * 2;

            ExecuteMarketOrder(tradeType, SymbolName, _volumeInUnits, Label, stopLossInPips, takeProfitInPips);
        }
    }
}