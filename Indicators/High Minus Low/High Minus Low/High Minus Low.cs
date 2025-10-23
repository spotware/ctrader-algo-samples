using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class HighMinusLow : Indicator
    {
        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            Result[index + Shift] = Math.Round(Bars.HighPrices[index] - Bars.LowPrices[index], Symbol.Digits);
        }
    }
}