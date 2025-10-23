using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None)]
    public class OnBalanceVolume : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
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

            if (Source[index] > Source[index - 1])
                Result[outputIndex] = Result[outputIndex - 1] + Bars.TickVolumes[index];
            else if (Source[index] < Source[index - 1])
                Result[outputIndex] = Result[outputIndex - 1] - Bars.TickVolumes[index];
            else
                Result[outputIndex] = Result[outputIndex - 1];
        }
    }
}