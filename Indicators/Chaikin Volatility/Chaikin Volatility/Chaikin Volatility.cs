using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None)]
    public class ChaikinVolatility : Indicator
    {
        private HighMinusLow _highMinusLow;
        private MovingAverage _ma;

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Rate of Change", DefaultValue = 10, MinValue = 0)]
        public int RateOfChange { get; set; }

        [Parameter("MA Type")]
        public MovingAverageType MAType { get; set; }

        [Output("Main", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _highMinusLow = Indicators.HighMinusLow();
            _ma = Indicators.MovingAverage(_highMinusLow.Result, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            var maRateOfChangeAgo = _ma.Result[index - RateOfChange];
            var maCurrent = _ma.Result[index];

            Result[index] = (maCurrent - maRateOfChangeAgo) / maRateOfChangeAgo * 100;
        }
    }
}