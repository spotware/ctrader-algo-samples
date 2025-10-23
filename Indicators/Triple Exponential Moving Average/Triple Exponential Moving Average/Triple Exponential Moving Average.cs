using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, ScalePrecision = 2, AutoRescale = false, AccessRights = AccessRights.None)]
    public class TripleExponentialMovingAverage : Indicator
    {
        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        private MovingAverage _ema1;
        private MovingAverage _ema2;
        private MovingAverage _ema3;

        protected override void Initialize()
        {
            _ema1 = Indicators.MovingAverage(Source, Periods, MovingAverageType.Exponential);
            _ema2 = Indicators.MovingAverage(_ema1.Result, Periods, MovingAverageType.Exponential);
            _ema3 = Indicators.MovingAverage(_ema2.Result, Periods, MovingAverageType.Exponential);
        }

        public override void Calculate(int index)
        {
            Result[index + Shift] = 3 * _ema1.Result[index] - 3 * _ema2.Result[index] + _ema3.Result[index];
        }
    }
}