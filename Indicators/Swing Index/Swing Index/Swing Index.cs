using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 3, AccessRights = AccessRights.None)]
    public class SwingIndex : Indicator
    {
        [Parameter("Limit Move Value", DefaultValue = 12, MinValue = 1)]
        public int LimitMoveValue { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var o2 = Bars.OpenPrices[index];
            var h2 = Bars.HighPrices[index];
            var l2 = Bars.LowPrices[index];
            var c2 = Bars.ClosePrices[index];

            var o1 = Bars.OpenPrices[index - 1];
            var c1 = Bars.ClosePrices[index - 1];

            var h2c1 = Math.Abs(h2 - c1);
            var l2c1 = Math.Abs(l2 - c1);
            var h2l2 = Math.Abs(h2 - l2);
            var c1o1 = Math.Abs(c1 - o1);

            double r;
            if (h2c1 > l2c1 && h2c1 > h2l2)
                r = h2c1 - 0.5 * l2c1 + 0.25 * c1o1;
            else if (l2c1 > h2c1 && l2c1 > h2l2)
                r = l2c1 - 0.5 * h2c1 + 0.25 * c1o1;
            else if (h2l2 > h2c1 && h2l2 > l2c1)
                r = h2l2 + 0.25 * c1o1;
            else
            {
                Result[index] = 0;
                return;
            }

            var k = Math.Max(h2c1, l2c1);
            Result[index] = 50 * (c2 - c1 + 0.5 * (c2 - o2) + 0.25 * (c1 - o1)) / r * k / LimitMoveValue;
        }
    }
}