using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class Aroon : Indicator
    {
        [Parameter(DefaultValue = 25, MinValue = 2)]
        public int Periods { get; set; }

        [Output("Up", LineColor = "Turquoise")]
        public IndicatorDataSeries Up { get; set; }

        [Output("Down", LineColor = "Red")]
        public IndicatorDataSeries Down { get; set; }

        public override void Calculate(int index)
        {
            if (index < Periods)
                return;

            var max = Bars.HighPrices[index];
            var min = Bars.LowPrices[index];
            var maxPeriod = 0;
            var minPeriod = 0;

            for (var p = 0; p < Periods; p++)
            {
                if (Bars.HighPrices[index - p] >= max)
                {
                    max = Bars.HighPrices[index - p];
                    maxPeriod = p;
                }

                if (Bars.LowPrices[index - p] <= min)
                {
                    min = Bars.LowPrices[index - p];
                    minPeriod = p;
                }
            }

            Up[index] = (Periods - maxPeriod - 1) * 100d / (Periods - 1);
            Down[index] = (Periods - minPeriod - 1) * 100d / (Periods - 1);
        }
    }
}