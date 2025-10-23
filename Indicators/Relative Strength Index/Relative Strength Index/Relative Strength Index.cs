using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, IsOverlay = false, AccessRights = AccessRights.None, IsPercentage = true)]
    [Levels(30, 70)]
    public class RelativeStrengthIndex : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        private IndicatorDataSeries _gains;
        private IndicatorDataSeries _losses;

        private MovingAverage _exponentialMovingAverageGain;
        private MovingAverage _exponentialMovingAverageLoss;

        protected override void Initialize()
        {
            _gains = CreateDataSeries();
            _losses = CreateDataSeries();
            var emaPeriods = 2 * Periods - 1;
            _exponentialMovingAverageGain = Indicators.MovingAverage(_gains, emaPeriods, MovingAverageType.Exponential);
            _exponentialMovingAverageLoss = Indicators.MovingAverage(_losses, emaPeriods, MovingAverageType.Exponential);
        }

        public override void Calculate(int index)
        {
            var currentValue = Source[index];
            var previousValue = Source[index - 1];

            if (currentValue > previousValue)
            {
                _gains[index] = currentValue - previousValue;
                _losses[index] = 0.0;
            }
            else if (currentValue < previousValue)
            {
                _gains[index] = 0.0;
                _losses[index] = previousValue - currentValue;
            }
            else
            {
                _gains[index] = 0.0;
                _losses[index] = 0.0;
            }

            var relativeStrength = _exponentialMovingAverageGain.Result[index] / _exponentialMovingAverageLoss.Result[index];

            Result[index] = 100 - 100 / (1 + relativeStrength);
        }
    }
}