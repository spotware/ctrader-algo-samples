using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    [Levels(0.0)]
    public class CyberCycle : Indicator
    {
        [Parameter(DefaultValue = 0.07, MinValue = 0.01, MaxValue = 100, Step = 0.01)]
        public double Alpha { get; set; }

        [Output("Cyber Cycle", LineColor = "Red")]
        public IndicatorDataSeries Cycle { get; set; }

        [Output("Trigger", LineColor = "Blue")]
        public IndicatorDataSeries Trigger { get; set; }

        private IndicatorDataSeries _medianPriceDataSeries;
        private IndicatorDataSeries _smoothDataSeries;

        protected override void Initialize()
        {
            _medianPriceDataSeries = Indicators.MedianPrice().Result;
            _smoothDataSeries = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            _smoothDataSeries[index] = (_medianPriceDataSeries[index] + 2 * _medianPriceDataSeries[index - 1] + 2 * _medianPriceDataSeries[index - 2] +
                                        _medianPriceDataSeries[index - 3]) / 6;

            Cycle[index] = (1 - 0.5 * Alpha) * (1 - 0.5 * Alpha) *
                           (_smoothDataSeries[index] - 2 * _smoothDataSeries[index - 1] + _smoothDataSeries[index - 2]) +
                           2 * (1 - Alpha) * Cycle[index - 1] -
                           (1 - Alpha) * (1 - Alpha) * Cycle[index - 2];

            if (index < 7)
                Cycle[index] = (_medianPriceDataSeries[index] - 2 * _medianPriceDataSeries[index - 1] + _medianPriceDataSeries[index - 2]) / 4;

            Trigger[index] = Cycle[index - 1];
        }
    }
}