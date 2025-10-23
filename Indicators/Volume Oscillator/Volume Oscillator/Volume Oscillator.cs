using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class VolumeOscillator : Indicator
    {
        private MovingAverage _fastMovingAverage;
        private MovingAverage _slowMovingAverage;

        [Parameter("Short Term", DefaultValue = 9, MinValue = 1)]
        public int ShortTerm { get; set; }

        [Parameter("Long Term", DefaultValue = 21, MinValue = 1)]
        public int LongTerm { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _fastMovingAverage = Indicators.MovingAverage(Bars.TickVolumes, ShortTerm, MovingAverageType.Simple);
            _slowMovingAverage = Indicators.MovingAverage(Bars.TickVolumes, LongTerm, MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            Result[index + Shift] = _fastMovingAverage.Result[index] - _slowMovingAverage.Result[index];
        }
    }
}