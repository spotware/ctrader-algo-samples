using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Linq;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, AccessRights = AccessRights.None)]
    public class LinearRegressionRSquared : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Output("RSquared", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 9, MinValue = 1)]
        public int Periods { get; set; }

        public override void Calculate(int index)
        {
            var lastPoints = Enumerable.Range(0, Periods)
                .Select(
                    i => new
                    {
                        X = i + 1,
                        Y = Source[index - Periods + i + 1],
                    })
                .ToArray();

            var Ex = lastPoints.Sum(point => point.X);
            var Ey = lastPoints.Sum(point => point.Y);
            var Ex2 = lastPoints.Sum(point => point.X * point.X);
            var Ey2 = lastPoints.Sum(point => point.Y * point.Y);
            var Exy = lastPoints.Sum(point => point.X * point.Y);

            Result[index] = Math.Pow(Exy * Periods - Ex * Ey, 2) / (Ex2 * Periods - Ex * Ex) / (Ey2 * Periods - Ey * Ey);
        }
    }
}