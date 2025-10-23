using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class BollingerBands : Indicator
    {
        private MovingAverage _movingAverage;
        private StandardDeviation _standardDeviation;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 20, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Standard Dev", DefaultValue = 2.0, MinValue = 0.0001, MaxValue = 10)]
        public double StandardDeviations { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Main { get; set; }

        [Output("Top")]
        public IndicatorDataSeries Top { get; set; }

        [Output("Bottom")]
        public IndicatorDataSeries Bottom { get; set; }

        protected override void Initialize()
        {
            _movingAverage = Indicators.MovingAverage(Source, Periods, MAType);
            _standardDeviation = Indicators.StandardDeviation(Source, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            var deviationShift = _standardDeviation.Result[index] * StandardDeviations;

            Main[outputIndex] = _movingAverage.Result[index];
            Bottom[outputIndex] = _movingAverage.Result[index] - deviationShift;
            Top[outputIndex] = _movingAverage.Result[index] + deviationShift;
        }
    }
}