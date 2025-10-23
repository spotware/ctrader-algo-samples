using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class LinearRegressionSlope : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 9, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Slope", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            if (index <= Periods)
                return;

            double x, y, ex = 0, ey = 0, ex2 = 0, exy = 0;

            for (var i = 0; i < Periods; i++)
            {
                x = i + 1;
                y = Source[index - Periods + i + 1];

                ex += x;
                ey += y;
                ex2 += x * x;
                exy += x * y;
            }

            Result[index] = (exy * Periods - ex * ey) / (ex2 * Periods - Math.Pow(ex, 2));
        }
    }
}