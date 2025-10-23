using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class WilliamsAccumulationDistribution : Indicator
    {
        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var trueRangeHigh = CalculateTrueRangeHigh(index);
            var trueRangeLow = CalculateTrueRangeLow(index);

            var value = CalculateValue(index, trueRangeLow, trueRangeHigh);

            var previousValue = double.IsNaN(Result[index - 1]) ? 0 : Result[index - 1];
            Result[index] = value + previousValue;
        }

        private double CalculateTrueRangeHigh(int index)
        {
            var trueHighRange = Bars.ClosePrices[index - 1];

            if (Bars.HighPrices[index] > trueHighRange)
                trueHighRange = Bars.HighPrices[index];

            return trueHighRange;
        }

        private double CalculateTrueRangeLow(int index)
        {
            var trueRangeLow = Bars.ClosePrices[index - 1];

            if (Bars.LowPrices[index] < trueRangeLow)
                trueRangeLow = Bars.LowPrices[index];

            return trueRangeLow;
        }

        private double CalculateValue(int index, double trueRangeLow, double trueHighRange)
        {
            if (Bars.ClosePrices[index] > Bars.ClosePrices[index - 1])
                return Bars.ClosePrices[index] - trueRangeLow;

            if (Bars.ClosePrices[index] < Bars.ClosePrices[index - 1])
                return Bars.ClosePrices[index] - trueHighRange;

            return 0;
        }
    }
}