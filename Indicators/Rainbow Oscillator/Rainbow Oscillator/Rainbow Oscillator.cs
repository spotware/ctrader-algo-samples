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
    public class RainbowOscillator : Indicator
    {
        private IEnumerable<MovingAverage> _indicators;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(MinValue = 2, DefaultValue = 9)]
        public int Levels { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            IEnumerable<MovingAverage> indicators;

            indicators = MAType == MovingAverageType.VIDYA
                ? Enumerable.Range(2, Levels - 1).Select(level => Indicators.MovingAverage(Source, level, MAType))
                : Enumerable.Range(0, Levels - 1).Select(_ => Indicators.MovingAverage(Source, Levels, MAType));

            _indicators = indicators.ToArray();
        }

        public override void Calculate(int index)
        {
            var aggregatedValue = double.NaN;
            foreach (var indicator in _indicators)
            {
                var currentValue = Source[index] - indicator.Result[index];

                if (double.IsNaN(aggregatedValue))
                    aggregatedValue = currentValue;
                else
                    aggregatedValue += currentValue;
            }

            var normalizedValue = aggregatedValue / Levels;
            Result[index] = normalizedValue;
        }
    }
}