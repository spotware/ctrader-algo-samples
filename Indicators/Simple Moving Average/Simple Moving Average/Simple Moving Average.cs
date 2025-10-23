using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class SimpleMovingAverage : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;

            double sum = 0;

            var startBarIndex = index - Periods + 1;

            for (var i = startBarIndex; i <= index; i++)
                sum += Source[i];

            Result[outputIndex] = sum / Periods;
        }
    }
}