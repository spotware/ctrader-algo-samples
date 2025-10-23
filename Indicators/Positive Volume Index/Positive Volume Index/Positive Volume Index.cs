using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class PositiveVolumeIndex : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 0, MinValue = 0, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            if (index == 0)
            {
                Result[outputIndex] = 1;
                return;
            }

            if (Bars.TickVolumes[index] <= Bars.TickVolumes[index - 1])
                Result[outputIndex] = Result[outputIndex - 1];
            else
                Result[outputIndex] = Result[outputIndex - 1] + (Source[index] - Source[index - 1]) / Source[index - 1] * Result[outputIndex - 1];
        }
    }
}