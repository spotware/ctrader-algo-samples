using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class TrueRange : Indicator
    {
        [Output("Main", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var high = Bars.HighPrices[index];
            var low = Bars.LowPrices[index];
            var previousClose = Bars.ClosePrices[index - 1];

            Result[index] = Math.Max(high, previousClose) - Math.Min(low, previousClose);
        }
    }
}