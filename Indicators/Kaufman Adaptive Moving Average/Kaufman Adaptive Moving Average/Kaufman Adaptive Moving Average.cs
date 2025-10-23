using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, ScalePrecision = 2, AutoRescale = false, AccessRights = AccessRights.None)]
    public class KaufmanAdaptiveMovingAverage : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 10, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 2, MinValue = 1)]
        public int FastPeriods { get; set; }

        [Parameter(DefaultValue = 30, MinValue = 1)]
        public int SlowPeriods { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        protected override void Initialize()
        {
            Source ??= Bars.ClosePrices;
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            var previousValue = Result[outputIndex - 1];

            if (index < Periods)
                return;

            if (double.IsNaN(previousValue))
            {
                Result[outputIndex] = Source[index];
                return;
            }

            var direction = Math.Abs(Source[index] - Source[index - Periods]);
            var volatility = Source.Skip(index - Periods)
                .Take(Periods)
                .Zip(Source.Skip(index - Periods - 1).Take(Periods), (f, s) => Math.Abs(f - s))
                .Sum();

            var er = direction / volatility;
            var fastSc = 2.0 / (FastPeriods + 1);
            var slowSc = 2.0 / (SlowPeriods + 1);
            var sc = Math.Pow(er * (fastSc - slowSc) + slowSc, 2);

            Result[outputIndex] = previousValue + sc * (Source[index] - previousValue);
        }
    }
}