using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, ScalePrecision = 2, AutoRescale = false, AccessRights = AccessRights.None)]
    public class FractalAdaptiveMovingAverage : Indicator
    {
        [Parameter(DefaultValue = 16, MinValue = 2, Step = 2)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        private int _halfPeriod;
        private IndicatorDataSeries _dimen;

        protected override void Initialize()
        {
            _halfPeriod = Periods / 2;
            _dimen = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            var previousValue = Result[outputIndex - 1];
            if (double.IsNaN(previousValue))
            {
                Result[outputIndex] = Bars.ClosePrices[index];
                _dimen[index] = 0;
                return;
            }

            var startBarIndex = index - Periods + 1;
            var n2 = N(_halfPeriod, startBarIndex);
            var n1 = N(_halfPeriod, startBarIndex + _halfPeriod);
            var n3 = N(Periods, startBarIndex);

            var d = _dimen[index - 1];

            if (n1 > 0 && n2 > 0 && n3 > 0)
                d = (Math.Log10(n1 + n2) - Math.Log10(n3)) / Math.Log10(2);

            _dimen[index] = d;

            var alpha = Math.Exp(-4.6 * (d - 1));
            alpha = Math.Min(alpha, 1);
            alpha = Math.Max(alpha, 0.01);

            Result[outputIndex] = Bars.ClosePrices[index] * alpha + previousValue * (1 - alpha);
        }

        private double N(int length, int index)
        {
            var data = Bars.Skip(index).Take(length).ToArray();
            var max = data.Max(b => b.High);
            var min = data.Min(b => b.Low);
            return (max - min) / length;
        }
    }
}