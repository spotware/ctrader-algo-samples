using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class DonchianChannel : Indicator
    {
        [Parameter(DefaultValue = 20, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Top", LineColor = "Red")]
        public IndicatorDataSeries Top { get; set; }

        [Output("Middle")]
        public IndicatorDataSeries Middle { get; set; }

        [Output("Bottom", LineColor = "Blue")]
        public IndicatorDataSeries Bottom { get; set; }

        public override void Calculate(int index)
        {
            if (index < Periods)
                return;

            var low = double.NaN;
            var high = double.NaN;
            for (var i = 1; i <= Periods; i++)
            {
                var currentLow = Bars.LowPrices[index - i];
                if (currentLow < low || double.IsNaN(low))
                    low = currentLow;

                var currentHigh = Bars.HighPrices[index - i];
                if (currentHigh > high || double.IsNaN(high))
                    high = currentHigh;
            }

            var outputIndex = index + Shift;
            Top[outputIndex] = high;
            Bottom[outputIndex] = low;
            Middle[outputIndex] = (high + low) / 2;
        }
    }
}