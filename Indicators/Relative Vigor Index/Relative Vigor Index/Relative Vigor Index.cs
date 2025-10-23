using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class RelativeVigorIndex : Indicator
    {
        private IndicatorDataSeries _numerator;
        private IndicatorDataSeries _denominator;
        private SimpleMovingAverage _numeratorSma;
        private SimpleMovingAverage _denominatorSma;

        [Parameter("Periods", DefaultValue = 10, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Result", LineColor = "Blue")]
        public IndicatorDataSeries Result { get; set; }

        [Output("Signal", LineColor = "Red", LineStyle = LineStyle.Lines)]
        public IndicatorDataSeries Signal { get; set; }

        protected override void Initialize()
        {
            _numerator = CreateDataSeries();
            _denominator = CreateDataSeries();
            _numeratorSma = Indicators.SimpleMovingAverage(_numerator, Periods);
            _denominatorSma = Indicators.SimpleMovingAverage(_denominator, Periods);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            if (index < 3)
            {
                Result[outputIndex] = double.NaN;
                return;
            }

            var a = Bars.ClosePrices[index] - Bars.OpenPrices[index];
            var b = Bars.ClosePrices[index - 1] - Bars.OpenPrices[index - 1];
            var c = Bars.ClosePrices[index - 2] - Bars.OpenPrices[index - 2];
            var d = Bars.ClosePrices[index - 3] - Bars.OpenPrices[index - 3];
            _numerator[index] = (a + 2 * b + 2 * c + d) / 6;

            var e = Bars.HighPrices[index] - Bars.LowPrices[index];
            var f = Bars.HighPrices[index - 1] - Bars.LowPrices[index - 1];
            var g = Bars.HighPrices[index - 2] - Bars.LowPrices[index - 2];
            var h = Bars.HighPrices[index - 3] - Bars.LowPrices[index - 3];
            _denominator[index] = (e + 2 * f + 2 * g + h) / 6;

            if (_denominatorSma.Result[index] == 0 || _denominatorSma.Result[index] == double.NaN)
            {
                Result[outputIndex] = double.NaN;
                return;
            }

            Result[outputIndex] = _numeratorSma.Result[index] / _denominatorSma.Result[index];
            Signal[outputIndex] = (Result[outputIndex] + 2 * Result[outputIndex - 1] + 2 * Result[outputIndex - 2] + Result[outputIndex - 3]) / 6;
        }
    }
}