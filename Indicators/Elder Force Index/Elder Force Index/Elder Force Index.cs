using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class ElderForceIndex : Indicator
    {
        private MovingAverage _ma;
        private IndicatorDataSeries _fi;

        [Parameter(DefaultValue = 13, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Result", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _fi = CreateDataSeries();
            _ma = Indicators.MovingAverage(_fi, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            if (index < 1)
            {
                Result[outputIndex] = double.NaN;
                return;
            }

            _fi[index] = (Bars.ClosePrices[index] - Bars.ClosePrices[index - 1]) * Bars.TickVolumes[index];
            Result[outputIndex] = _ma.Result[index];
        }
    }
}