using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, ScalePrecision = 2, AutoRescale = false, AccessRights = AccessRights.None)]
    public class TriangularMovingAverage : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        private SimpleMovingAverage _movingAverageOverSource;
        private SimpleMovingAverage _movingAverageOverMovingAverage;

        protected override void Initialize()
        {
            _movingAverageOverSource = (SimpleMovingAverage)Indicators.MovingAverage(Source, Periods, MovingAverageType.Simple);
            _movingAverageOverMovingAverage = (SimpleMovingAverage)Indicators.MovingAverage(
                _movingAverageOverSource.Result,
                Periods,
                MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            Result[index + Shift] = _movingAverageOverMovingAverage.Result[index];
        }
    }
}