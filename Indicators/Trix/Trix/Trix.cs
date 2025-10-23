using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 3, AccessRights = AccessRights.None)]
    public class Trix : Indicator
    {
        private ExponentialMovingAverage _singleSmoothed;
        private ExponentialMovingAverage _doubleSmoothed;
        private ExponentialMovingAverage _tripleSmoothed;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 8, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _singleSmoothed = Indicators.ExponentialMovingAverage(Source, Periods);
            _doubleSmoothed = Indicators.ExponentialMovingAverage(_singleSmoothed.Result, Periods);
            _tripleSmoothed = Indicators.ExponentialMovingAverage(_doubleSmoothed.Result, Periods);
        }

        public override void Calculate(int index)
        {
            var previous = _tripleSmoothed.Result[index - 1];
            var current = _tripleSmoothed.Result[index];

            Result[index] = (current - previous) / previous * 100;
        }
    }
}