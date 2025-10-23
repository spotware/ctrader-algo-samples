using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 2, IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class Envelopes : Indicator
    {
        private MovingAverage _movingAverage;

        [Output("Upper", LineColor = "#B268BCFF")]
        public IndicatorDataSeries Upper { get; set; }

        [Output("Main", LineColor = "#B2B38AB0")]
        public IndicatorDataSeries Main { get; set; }

        [Output("Lower", LineColor = "#B2FF5861")]
        public IndicatorDataSeries Lower { get; set; }

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Parameter(DefaultValue = 0.2)]
        public double Deviation { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        protected override void Initialize()
        {
            Source ??= Bars.ClosePrices;
            _movingAverage = Indicators.MovingAverage(Source, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            var maValue = _movingAverage.Result[index];
            Main[outputIndex] = maValue;
            Upper[outputIndex] = maValue * (1 + Deviation / 100);
            Lower[outputIndex] = maValue * (1 - Deviation / 100);
        }
    }
}