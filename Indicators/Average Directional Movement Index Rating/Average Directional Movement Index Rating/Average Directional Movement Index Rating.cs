using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None, IsPercentage = true)]
    public class AverageDirectionalMovementIndexRating : Indicator
    {
        private DirectionalMovementSystem _dms;

        [Parameter(DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("ADX", LineColor = "Turquoise")]
        public IndicatorDataSeries ADX { get; set; }

        [Output("ADXR", LineColor = "Gold")]
        public IndicatorDataSeries ADXR { get; set; }

        [Output("DI-", LineColor = "Red")]
        public IndicatorDataSeries DIMinus { get; set; }

        [Output("DI+", LineColor = "Green")]
        public IndicatorDataSeries DIPlus { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.WilderSmoothing)]
        public MovingAverageType MAType { get; set; }

        protected override void Initialize()
        {
            _dms = Indicators.DirectionalMovementSystem(Bars, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            ADX[index] = _dms.ADX[index];
            ADXR[index] = (_dms.ADX[index] + _dms.ADX[index - Periods]) / 2;
            DIMinus[index] = _dms.DIMinus[index];
            DIPlus[index] = _dms.DIPlus[index];
        }
    }
}