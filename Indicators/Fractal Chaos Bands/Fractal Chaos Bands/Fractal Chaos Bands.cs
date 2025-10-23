using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class FractalChaosBands : Indicator
    {
        private enum PeakType
        {
            None,
            Low,
            High,
        }

        private IndicatorDataSeries _highFractal;

        private double _high1;
        private double _high2;
        private double _high3;
        private double _high4;

        private double _low1;
        private double _low2;
        private double _low3;
        private double _low4;

        private PeakType _currentPeak;

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Low", LineColor = "Orange")]
        public IndicatorDataSeries Low { get; set; }

        [Output("High", LineColor = "Green")]
        public IndicatorDataSeries High { get; set; }

        protected override void Initialize()
        {
            _highFractal = CreateDataSeries();
        }

        private void CalculateLowBy(int i)
        {
            _low1 = Bars.LowPrices[i - 4];
            _low2 = Bars.LowPrices[i - 3];
            _low3 = Bars.LowPrices[i - 2];
            _low4 = Bars.LowPrices[i - 1];
        }

        private void CalculateHighBy(int i)
        {
            _high1 = Bars.HighPrices[i - 4];
            _high2 = Bars.HighPrices[i - 3];
            _high3 = Bars.HighPrices[i - 2];
            _high4 = Bars.HighPrices[i - 1];
        }

        public override void Calculate(int index)
        {
            _highFractal[index] = (Bars.HighPrices[index] + Bars.LowPrices[index]) / 3;

            CalculateHighBy(index);
            CalculateLowBy(index);

            var outputIndex = index + Shift;

            High[outputIndex] = _high3;
            Low[outputIndex] = _low3;

            CalculateFrBy(index);
            AdjustFractalsWithFrBy(outputIndex);
        }

        private void AdjustFractalsWithFrBy(int i)
        {
            if (_currentPeak == PeakType.High)
                High[i] = _high3;
            else
                High[i] = High[i - 1];

            if (_currentPeak == PeakType.Low)
                Low[i] = _low3;
            else
                Low[i] = Low[i - 1];
        }

        private void CalculateFrBy(int i)
        {
            if (_high3 > _high1 && _high3 > _high2 && _high3 >= _high4 && _high3 >= Bars.HighPrices[i])
                _currentPeak = PeakType.High;
            else if (_low3 < _low1 && _low3 < _low2 && _low3 <= _low4 && _low3 <= Bars.LowPrices[i])
                _currentPeak = PeakType.Low;
            else
                _currentPeak = PeakType.None;
        }
    }
}