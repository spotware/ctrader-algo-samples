using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class TimeSeriesMovingAverage : Indicator
    {
        private LinearRegressionForecast _linearRegressionForecast;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 2)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _linearRegressionForecast = Indicators.LinearRegressionForecast(Source, Periods);
        }

        public override void Calculate(int index)
        {
            Result[index + Shift] = _linearRegressionForecast.Result[index];
        }
    }
}