using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class WellesWilderSmoothing : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1, MaxValue = 10000)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            var previousValue = Result[outputIndex - 1];

            if (double.IsNaN(previousValue))
                Result[outputIndex] = Source[index];
            else
                Result[outputIndex] = previousValue + (Source[index] - previousValue) / Periods;
        }
    }
}