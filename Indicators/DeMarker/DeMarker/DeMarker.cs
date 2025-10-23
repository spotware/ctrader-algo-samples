using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class DeMarker : Indicator
    {
        private IndicatorDataSeries _deMax;
        private IndicatorDataSeries _deMin;
        private SimpleMovingAverage _deMaxSma;
        private SimpleMovingAverage _deMinSma;

        [Parameter("Periods", DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Result", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _deMax = CreateDataSeries();
            _deMin = CreateDataSeries();

            _deMaxSma = Indicators.SimpleMovingAverage(_deMax, Periods);
            _deMinSma = Indicators.SimpleMovingAverage(_deMin, Periods);
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;
            if (index < 1)
            {
                Result[outputIndex] = double.NaN;
                return;
            }

            _deMax[index] = Bars.HighPrices[index] > Bars.HighPrices[index - 1] ? Bars.HighPrices[index] - Bars.HighPrices[index - 1] : 0;
            _deMin[index] = Bars.LowPrices[index] < Bars.LowPrices[index - 1] ? Bars.LowPrices[index - 1] - Bars.LowPrices[index] : 0;


            if (_deMaxSma.Result[index] + _deMinSma.Result[index] == 0)
            {
                Result[outputIndex] = double.NaN;
                return;
            }

            Result[outputIndex] = _deMaxSma.Result[index] / (_deMaxSma.Result[index] + _deMinSma.Result[index]);
        }
    }
}