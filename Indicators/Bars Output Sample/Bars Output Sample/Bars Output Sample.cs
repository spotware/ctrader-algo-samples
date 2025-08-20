using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class BarsOutputSample : Indicator
    {
        private ExponentialMovingAverage _openEma;
        private ExponentialMovingAverage _highEma;
        private ExponentialMovingAverage _lowEma;
        private ExponentialMovingAverage _closeEma;
        
        [Parameter("EMA Periods", DefaultValue = 9)]
        public int EmaPeriods { get; set; }

        [BarOutput("Main")]
        public IndicatorBars Result { get; set; }

        protected override void Initialize()
        {
            _openEma = Indicators.ExponentialMovingAverage(Bars.OpenPrices, EmaPeriods);
            _highEma = Indicators.ExponentialMovingAverage(Bars.HighPrices, EmaPeriods);
            _lowEma = Indicators.ExponentialMovingAverage(Bars.LowPrices, EmaPeriods);
            _closeEma = Indicators.ExponentialMovingAverage(Bars.ClosePrices, EmaPeriods);

            // Hide main chart bars
            Chart.DisplaySettings.Bars = false;

            // You can change output colors and other appearance properties via code
            // Result.Output.BullFillColor = Color.Blue;
            // Result.Output.BullOutlineColor = Color.Blue;
            //
            // Result.Output.BearFillColor = Color.Gold;
            // Result.Output.BearOutlineColor = Color.Gold;
            //
            // Result.Output.ChartType = ChartType.Bars;
            //
            // Result.Output.TickVolumeColor = Color.Magenta;
        }

        public override void Calculate(int index)
        {
            Result[index] = new (
                _openEma.Result[index],
                _highEma.Result[index],
                _lowEma.Result[index],
                _closeEma.Result[index],
                Convert.ToInt64(Bars.TickVolumes[index]));
        }
    }
}