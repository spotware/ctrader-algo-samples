using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None)]
    [Levels(-100, 100)]
    public class CommodityChannelIndex : Indicator
    {
        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter(DefaultValue = "Typical")]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 20, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _simpleMovingAverage = Indicators.SimpleMovingAverage(Source, Periods);
        }

        public override void Calculate(int index)
        {
            if (index < 2 * Periods)
                return;

            double meanDeviation = 0;

            var simpleMaValue = _simpleMovingAverage.Result[index];

            for (var count = index - Periods; count < index; ++count)
            {
                meanDeviation += Math.Abs(Source[count] - simpleMaValue);
            }

            meanDeviation /= Periods;
            Result[index + Shift] = (Source[index] - simpleMaValue) / (meanDeviation * 0.015);
        }
    }
}