using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class ElderRayIndex : Indicator
    {
        private MovingAverage _ma;

        [Parameter(DefaultValue = 13, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("BearsPower", LineColor = "#f0937b", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries BearsPower { get; set; }

        [Output("BullsPower", LineColor = "#8cc44c", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries BullsPower { get; set; }

        protected override void Initialize()
        {
            _ma = Indicators.MovingAverage(Bars.ClosePrices, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            BearsPower[outputIndex] = Bars.LowPrices[index] - _ma.Result[index];
            BullsPower[outputIndex] = Bars.HighPrices[index] - _ma.Result[index];
        }
    }
}