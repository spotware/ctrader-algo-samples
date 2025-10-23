using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 3, AccessRights = AccessRights.None)]
    public class PriceOscillator : Indicator
    {
        private MovingAverage _longMa;
        private MovingAverage _shortMa;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter("Long Cycle", DefaultValue = 22, MinValue = 1)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", DefaultValue = 14, MinValue = 1)]
        public int ShortCycle { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Output("Main", LineColor = "Green")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _longMa = Indicators.MovingAverage(Source, LongCycle, MAType);
            _shortMa = Indicators.MovingAverage(Source, ShortCycle, MAType);
        }

        public override void Calculate(int index)
        {
            var shortValue = _shortMa.Result[index];
            var longValue = _longMa.Result[index];

            Result[index] = (shortValue - longValue) / longValue * 100;
        }
    }
}