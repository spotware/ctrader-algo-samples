using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class AverageTrueRange : Indicator
    {
        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Output("Main", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        private MovingAverage _movingAverage;

        protected override void Initialize()
        {
            var trueRange = Indicators.TrueRange();
            _movingAverage = Indicators.MovingAverage(trueRange.Result, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            Result[index] = _movingAverage.Result[index];
        }
    }
}