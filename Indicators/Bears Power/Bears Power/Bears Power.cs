using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class BearsPower : Indicator
    {
        private MovingAverage _ma;

        [Parameter(DefaultValue = 13, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "#f0937b", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _ma = Indicators.MovingAverage(Bars.ClosePrices, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            Result[outputIndex] = Bars.LowPrices[index] - _ma.Result[index];
        }
    }
}