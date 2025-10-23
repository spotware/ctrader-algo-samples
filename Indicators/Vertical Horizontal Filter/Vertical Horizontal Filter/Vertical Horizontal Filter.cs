using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, AccessRights = AccessRights.None)]
    public class VerticalHorizontalFilter : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        public override void Calculate(int index)
        {
            var max = double.MinValue;
            var min = double.MaxValue;
            double sum = 0;

            for (var i = index - Periods + 1; i <= index; i++)
            {
                max = Math.Max(max, Source[i]);
                min = Math.Min(min, Source[i]);
                sum += Math.Abs(Source[i] - Source[i - 1]);
            }

            Result[index] = Math.Abs((max - min) / sum);
        }
    }
}