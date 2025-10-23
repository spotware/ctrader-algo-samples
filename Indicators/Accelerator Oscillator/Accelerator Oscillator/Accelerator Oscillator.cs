using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class AcceleratorOscillator : Indicator
    {
        private AwesomeOscillator _awesomeOscillator;
        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Up", DefaultValue = "Green")]
        public Color UpColor { get; set; }

        [Parameter("Down", DefaultValue = "Red")]
        public Color DownColor { get; set; }

        [Output("Result", PlotType = PlotType.Histogram, IsColorCustomizable = false)]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _awesomeOscillator = Indicators.AwesomeOscillator();
            _simpleMovingAverage = Indicators.SimpleMovingAverage(_awesomeOscillator.Result, 5);
        }

        public override void Calculate(int index)
        {
            Result[index] = _awesomeOscillator.Result[index] - _simpleMovingAverage.Result[index];

            SetLineAppearance(Result.LineOutput, index, 1, Result[index] > Result[index - 1] ? UpColor : DownColor);
        }
    }
}