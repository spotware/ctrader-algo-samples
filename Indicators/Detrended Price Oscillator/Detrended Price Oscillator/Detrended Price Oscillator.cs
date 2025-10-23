using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class DetrendedPriceOscillator : Indicator
    {
        private MovingAverage _ma;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 21, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type")]
        public MovingAverageType MAType { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _ma = Indicators.MovingAverage(Source, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            Result[index] = Source[index] - _ma.Result[index - Periods / 2 - 1];
        }
    }
}