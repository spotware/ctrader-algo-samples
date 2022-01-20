using cAlgo.API;
using cAlgo.API.Indicators;
using System;
using System.Linq;

namespace cAlgo.Robots
{
    // This sample cBot shows how to use the Williams Accumulation Distribution indicator
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WilliamsAccumulationDistributionSample : Robot
    {
        private double _volumeInUnits;

        private WilliamsAccumulationDistribution _williamsAccumulationDistribution;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, Group = "Trade")]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, Group = "Trade")]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "Sample", Group = "Trade")]
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

            _williamsAccumulationDistribution = Indicators.WilliamsAccumulationDistribution();

            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
        }

        protected override void OnBar()
        {
            var correlation = GetCorrelation(14);

            if (correlation > 0.85) return;

            if (Bars.ClosePrices.Last(1) > _simpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (Bars.ClosePrices.Last(1) < _simpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
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

        private double GetCorrelation(int period)
        {
            var x = _williamsAccumulationDistribution.Result.Skip(_williamsAccumulationDistribution.Result.Count - period).ToArray();
            var y = Bars.ClosePrices.Skip(Bars.ClosePrices.Count - period).ToArray();

            if (!x.Any() || !y.Any())
            {
                return double.NaN;
            }

            var xSum = x.Sum();
            var ySum = y.Sum();

            var xSumSquared = Math.Pow(xSum, 2);
            var ySumSquared = Math.Pow(ySum, 2);

            var xSquaredSum = x.Select(value => Math.Pow(value, 2)).Sum();
            var ySquaredSum = y.Select(value => Math.Pow(value, 2)).Sum();

            var xAndyProductSum = x.Zip(y, (value1, value2) => value1 * value2).Sum();

            double n = x.Count();

            return (n * xAndyProductSum - xSum * ySum) / Math.Sqrt((n * xSquaredSum - xSumSquared) * (n * ySquaredSum - ySumSquared));
        }
    }
}