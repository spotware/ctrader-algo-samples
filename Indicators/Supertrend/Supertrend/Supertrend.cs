using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class Supertrend : Indicator
    {
        [Parameter(DefaultValue = 10, MinValue = 1, MaxValue = 2000)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 3.0)]
        public double Multiplier { get; set; }

        [Parameter(DefaultValue = 0, MinValue = 0, MaxValue = 200)]
        public int Shift { get; set; }

        [Output("Up Trend", LineColor = "Green", PlotType = PlotType.Points, Thickness = 2)]
        public IndicatorDataSeries UpTrend { get; set; }

        [Output("Down Trend", LineColor = "Red", PlotType = PlotType.Points, Thickness = 2)]
        public IndicatorDataSeries DownTrend { get; set; }

        private IndicatorDataSeries _upBuffer;
        private IndicatorDataSeries _downBuffer;
        private AverageTrueRange _averageTrueRange;
        private IndicatorDataSeries _trend;

        protected override void Initialize()
        {
            _trend = CreateDataSeries();
            _upBuffer = CreateDataSeries();
            _downBuffer = CreateDataSeries();
            _averageTrueRange = Indicators.AverageTrueRange(Periods, MovingAverageType.Simple);
        }

        private void InitDataSeries(int index)
        {
            UpTrend[index + Shift] = double.NaN;
            DownTrend[index + Shift] = double.NaN;
        }

        private void CalculateSuperTrendLogic(int index, double median, double averageTrueRangeValue)
        {
            if (Bars.ClosePrices[index] > _upBuffer[index - 1])
                _trend[index] = 1;
            else if (Bars.ClosePrices[index] < _downBuffer[index - 1])
                _trend[index] = -1;
            else
                _trend[index] = _trend[index - 1] switch
                {
                    1 => 1,
                    -1 => -1,
                    _ => _trend[index],
                };

            _upBuffer[index] = _trend[index] switch
            {
                < 0 when _trend[index - 1] > 0 => median + Multiplier * averageTrueRangeValue,
                < 0 when _upBuffer[index] > _upBuffer[index - 1] => _upBuffer[index - 1],
                _ => _upBuffer[index],
            };

            _downBuffer[index] = _trend[index] switch
            {
                > 0 when _trend[index - 1] < 0 => median - Multiplier * averageTrueRangeValue,
                > 0 when _downBuffer[index] < _downBuffer[index - 1] => _downBuffer[index - 1],
                _ => _downBuffer[index],
            };
        }

        private void DrawIndicator(int index)
        {
            switch (_trend[index])
            {
                case 1:
                    UpTrend[index + Shift] = _downBuffer[index];
                    break;
                case -1:
                    DownTrend[index + Shift] = _upBuffer[index];
                    break;
            }
        }

        public override void Calculate(int index)
        {
            InitDataSeries(index);

            var median = (Bars.HighPrices[index] + Bars.LowPrices[index]) / 2;
            var averageTrueRangeValue = _averageTrueRange.Result[index];

            _upBuffer[index] = median + Multiplier * averageTrueRangeValue;
            _downBuffer[index] = median - Multiplier * averageTrueRangeValue;

            if (index < 1)
            {
                _trend[index] = 1;
                return;
            }

            CalculateSuperTrendLogic(index, median, averageTrueRangeValue);

            DrawIndicator(index);
        }
    }
}