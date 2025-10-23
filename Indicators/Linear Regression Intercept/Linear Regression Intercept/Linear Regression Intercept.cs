using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class LinearRegressionIntercept : Indicator
    {
        private LinearRegressionSlope _slope;
        private SimpleMovingAverage _sma;

        [Parameter]
        public DataSeries Source { get; set; }

        [Output("Intercept", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 9, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        protected override void Initialize()
        {
            _slope = Indicators.LinearRegressionSlope(Source, Periods);
            _sma = Indicators.SimpleMovingAverage(Source, Periods);
        }

        public override void Calculate(int index)
        {
            Result[index + Shift] = _sma.Result[index] - _slope.Result[index] * Math.Floor(Periods * 1.0 / 2);
        }
    }
}