using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, ScalePrecision = 2, AutoRescale = false, AccessRights = AccessRights.None)]
    public class DoubleExponentialMovingAverage : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        private MovingAverage _movingAverageOverSource;
        private MovingAverage _movingAverageOverMovingAverage;

        protected override void Initialize()
        {
            _movingAverageOverSource = Indicators.MovingAverage(Source, Periods, MovingAverageType.Exponential);
            _movingAverageOverMovingAverage = Indicators.MovingAverage(_movingAverageOverSource.Result, Periods, MovingAverageType.Exponential);
        }

        public override void Calculate(int index)
        {
            Result[index + Shift] = 2 * _movingAverageOverSource.Result[index] - _movingAverageOverMovingAverage.Result[index];
        }
    }
}