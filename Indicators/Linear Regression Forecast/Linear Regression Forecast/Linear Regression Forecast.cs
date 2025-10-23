using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class LinearRegressionForecast : Indicator
    {
        private LinearRegressionSlope _slope;
        private LinearRegressionIntercept _intercept;

        [Parameter]
        public DataSeries Source { get; set; }

        [Output("Forecast", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 9, MinValue = 0)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        protected override void Initialize()
        {
            _slope = Indicators.LinearRegressionSlope(Source, Periods);
            _intercept = Indicators.LinearRegressionIntercept(Source, Periods);
        }

        public override void Calculate(int index)
        {
            Result[index + Shift] = Periods * _slope.Result[index] + _intercept.Result[index];
        }
    }
}