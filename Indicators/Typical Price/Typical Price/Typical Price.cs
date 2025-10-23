using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class TypicalPrice : Indicator
    {
        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            Result[index] = (Bars.HighPrices[index] + Bars.LowPrices[index] + Bars.ClosePrices[index]) / 3;
        }
    }
}