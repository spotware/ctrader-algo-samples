using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class EaseOfMovement : Indicator
    {
        private IndicatorDataSeries _notSmoothedEaseOfMovement;

        private MovingAverage _movingAverage;

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _notSmoothedEaseOfMovement = CreateDataSeries();
            _movingAverage = Indicators.MovingAverage(_notSmoothedEaseOfMovement, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            if (index < 2)
                return;

            var high = Bars.HighPrices[index];
            var low = Bars.LowPrices[index];
            var prevHigh = Bars.HighPrices[index - 1];
            var prevLow = Bars.LowPrices[index - 1];
            var tickVolume = Bars.TickVolumes[index];

            var distanceMoved = (high + low) / 2 - (prevHigh + prevLow) / 2;

            double emv = 0;

            if (high != low)
                emv = distanceMoved / (tickVolume / (high - low));

            _notSmoothedEaseOfMovement[index] = emv * 10000;
            Result[index + Shift] = _movingAverage.Result[index];
        }
    }
}