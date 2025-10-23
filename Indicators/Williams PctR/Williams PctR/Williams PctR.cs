using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None)]
    public class WilliamsPctR : Indicator
    {
        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var highest = Bars.HighPrices[index];
            var lowest = Bars.LowPrices[index];

            for (var i = index - Periods + 1; i <= index; i++)
            {
                highest = Math.Max(highest, Bars.HighPrices[i]);
                lowest = Math.Min(lowest, Bars.LowPrices[i]);
            }

            Result[index] = (highest - Bars.ClosePrices[index]) / (highest - lowest) * -100;
        }
    }
}