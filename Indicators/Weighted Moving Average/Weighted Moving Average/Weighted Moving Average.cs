using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Linq;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class WeightedMovingAverage : Indicator
    {
        private int _weight;

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        protected override void Initialize()
        {
            _weight = Enumerable.Range(1, Periods).Sum();
        }

        public override void Calculate(int index)
        {
            var total = 0.0;
            var j = index;

            for (var period = Periods; period > 0; period--)
            {
                total += period * Source[j];
                j--;
            }

            Result[index + Shift] = total / _weight;
        }
    }
}