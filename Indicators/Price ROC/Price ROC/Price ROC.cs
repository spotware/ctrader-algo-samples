using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, AccessRights = AccessRights.None)]
    public class PriceROC : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 12, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            Result[index] = (Source[index] - Source[index - Periods]) / Source[index - Periods] * 100;
        }
    }
}