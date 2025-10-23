using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class PriceVolumeTrend : Indicator
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
                Result[outputIndex] = 0;
                return;
            }

            Result[outputIndex] = Result[outputIndex - 1] + Bars.TickVolumes[index] * (Source[index] - Source[index - 1]) / Source[index - 1];
        }
    }
}