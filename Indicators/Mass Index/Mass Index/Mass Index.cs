using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class MassIndex : Indicator
    {
        private MovingAverage _movingAverage1;
        private MovingAverage _movingAverage2;
        private HighMinusLow _highMinusLow;

        [Parameter(DefaultValue = 9, MinValue = 4)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _highMinusLow = Indicators.HighMinusLow();
            _movingAverage1 = Indicators.ExponentialMovingAverage(_highMinusLow.Result, 9);
            _movingAverage2 = Indicators.ExponentialMovingAverage(_movingAverage1.Result, 9);
        }

        public override void Calculate(int index)
        {
            var firstAverageSeries = GetPeriod(_movingAverage1.Result, index + 1, Periods + 1).Take(Periods);
            var secondAverageSeries = GetPeriod(_movingAverage2.Result, index + 1, Periods + 1).Take(Periods);

            var sum = firstAverageSeries.Zip(secondAverageSeries, (first, second) => first / second).Sum();
            Result[index] = sum;
        }

        private static IEnumerable<double> GetPeriod(IndicatorDataSeries values, int index, int periods)
        {
            return Enumerable.Range(index + 1 - periods, periods).Select(indx => values[indx]);
        }
    }
}