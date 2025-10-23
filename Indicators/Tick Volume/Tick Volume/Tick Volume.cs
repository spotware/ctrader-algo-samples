using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None)]
    public class TickVolume : Indicator
    {
        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "#2c6dc1", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;

            Result[outputIndex] = Bars.TickVolumes[index];
        }
    }
}