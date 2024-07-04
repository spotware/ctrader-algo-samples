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
using System;
using System.Linq;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class WilliamsAccumulationDistributionSample : Robot
    {
        private double _volumeInUnits;

        private WilliamsAccumulationDistribution _williamsAccumulationDistribution;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, Group = "Trade", MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, Group = "Trade", MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "WilliamsAccumulationDistributionSample", Group = "Trade")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceMovingAverage { get; set; }

        [Parameter("Periods Moving Average", DefaultValue = 14, Group = "Simple Moving Average", MinValue = 2)]
        public int PeriodsMovingAverage { get; set; }

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

            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceMovingAverage, PeriodsMovingAverage);
        }

        protected override void OnBarClosed()
        {
            var correlation = GetCorrelation(14);

            if (correlation > 0.85) return;

            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0))
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