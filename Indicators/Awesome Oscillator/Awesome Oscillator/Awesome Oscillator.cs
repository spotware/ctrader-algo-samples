using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class AwesomeOscillator : Indicator
    {
        [Parameter("Up", DefaultValue = "Green")]
        public Color UpColor { get; set; }

        [Parameter("Down", DefaultValue = "Red")]
        public Color DownColor { get; set; }

        [Output("Result", PlotType = PlotType.Histogram, IsColorCustomizable = false)]
        public IndicatorDataSeries Result { get; set; }

        private SimpleMovingAverage _sma5;
        private SimpleMovingAverage _sma34;

        protected override void Initialize()
        {
            _sma5 = Indicators.SimpleMovingAverage(Bars.MedianPrices, 5);
            _sma34 = Indicators.SimpleMovingAverage(Bars.MedianPrices, 34);
        }

        public override void Calculate(int index)
        {
            Result[index] = _sma5.Result[index] - _sma34.Result[index];

            SetLineAppearance(Result.LineOutput, index, 1, Result[index] > Result[index - 1] ? UpColor : DownColor);
        }
    }
}