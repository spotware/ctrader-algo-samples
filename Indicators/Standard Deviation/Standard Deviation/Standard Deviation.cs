using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class StandardDeviation : Indicator
    {
        private MovingAverage _movingAverage;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 2)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Output("Main", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _movingAverage = Indicators.MovingAverage(Source, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            double sum = 0;

            var value = _movingAverage.Result[index];

            for (var period = 0; period < Periods; period++)
                sum += Math.Pow(Source[index - period] - value, 2.0);

            Result[index] = Math.Sqrt(sum / Periods);
        }
    }
}