// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Automate API example.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AutoRescale = false, AccessRights = AccessRights.None)]
    public class SamplePriceChannels : Indicator
    {
        [Parameter(DefaultValue = 20)]
        public int Periods { get; set; }

        [Output("Upper", LineColor = "Pink")]
        public IndicatorDataSeries Upper { get; set; }

        [Output("Lower", LineColor = "Pink")]
        public IndicatorDataSeries Lower { get; set; }

        [Output("Center", LineColor = "Pink")]
        public IndicatorDataSeries Center { get; set; }

        public override void Calculate(int index)
        {
            double upper = double.MinValue;
            double lower = double.MaxValue;

            for (int i = index - Periods; i <= index - 1; i++)
            {
                upper = Math.Max(Bars.HighPrices[i], upper);
                lower = Math.Min(Bars.LowPrices[i], lower);
            }

            Upper[index] = upper;
            Lower[index] = lower;
            Center[index] = (upper + lower) / 2;
        }
    }
}
