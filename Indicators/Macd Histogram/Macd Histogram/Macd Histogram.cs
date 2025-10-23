using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class MacdHistogram : Indicator
    {
        private ExponentialMovingAverage _emaLong;
        private ExponentialMovingAverage _emaShort;
        private ExponentialMovingAverage _emaSignal;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter("Long Cycle", DefaultValue = 26, MinValue = 1)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", DefaultValue = 12, MinValue = 1)]
        public int ShortCycle { get; set; }

        [Parameter("Signal Periods", DefaultValue = 9, MinValue = 1)]
        public int SignalPeriods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Turquoise", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Histogram { get; set; }

        [Output("Signal", LineColor = "Red", LineStyle = LineStyle.Lines)]
        public IndicatorDataSeries Signal { get; set; }

        protected override void Initialize()
        {
            _emaLong = Indicators.ExponentialMovingAverage(Source, LongCycle);
            _emaShort = Indicators.ExponentialMovingAverage(Source, ShortCycle);
            _emaSignal = Indicators.ExponentialMovingAverage(Histogram, SignalPeriods);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            Histogram[outputIndex] = _emaShort.Result[index] - _emaLong.Result[index];
            Signal[outputIndex] = _emaSignal.Result[index];
        }
    }
}