using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class VariableIndexDynamicAverage : Indicator
    {
        private ChandeMomentumOscillator _cmo;
        private double _smoothingFactor;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int CMOPeriods { get; set; }

        [Parameter(DefaultValue = 2, MinValue = 1)]
        public double SmoothPeriods { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            Source ??= Bars.ClosePrices;
            _cmo = Indicators.ChandeMomentumOscillator(Source, CMOPeriods);
            _smoothingFactor = 2 / (SmoothPeriods + 1);
        }

        public override void Calculate(int index)
        {
            var prevResult = Result[index - 1];

            if (double.IsNaN(prevResult))
                prevResult = Source[index - 1];

            var absCmo = Math.Abs(_cmo.Result[index]) / 100;
            Result[index] = Source[index] * absCmo * _smoothingFactor + prevResult * (1 - _smoothingFactor * absCmo);
        }
    }
}