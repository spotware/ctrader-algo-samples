using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = false, AccessRights = AccessRights.None)]
    public class UltimateOscillator : Indicator
    {
        [Parameter("Cycle 1", DefaultValue = 7, MinValue = 1)]
        public int Cycle1 { get; set; }

        [Parameter("Cycle 2", DefaultValue = 14, MinValue = 1)]
        public int Cycle2 { get; set; }

        [Parameter("Cycle 3", DefaultValue = 28, MinValue = 1)]
        public int Cycle3 { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var average1 = CalculateAverageBuyingPressure(index, Cycle1);
            var average2 = CalculateAverageBuyingPressure(index, Cycle2);
            var average3 = CalculateAverageBuyingPressure(index, Cycle3);

            Result[index] = (4 * average1 + 2 * average2 + average3) / (4 + 2 + 1) * 100;
        }

        private double CalculateAverageBuyingPressure(int index, int periods)
        {
            var totalBuyingPressure = 0d;
            var totalTrueRange = 0d;
            for (var i = periods - 1; i >= 0; i--)
            {
                var buyingPressure = CalculateBuyingPressure(index - i);

                var trueRange = CalculateTrueRange(index - i);

                totalBuyingPressure += buyingPressure;
                totalTrueRange += trueRange;
            }

            return totalBuyingPressure / totalTrueRange;
        }

        private double CalculateTrueRange(int period)
        {
            return Math.Max(Bars.HighPrices[period], Bars.ClosePrices[period - 1]) -
                   Math.Min(Bars.LowPrices[period], Bars.ClosePrices[period - 1]);
        }

        private double CalculateBuyingPressure(int period)
        {
            return Bars.ClosePrices[period] - Math.Min(Bars.LowPrices[period], Bars.ClosePrices[period - 1]);
        }
    }
}