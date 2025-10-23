using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None, IsPercentage = true)]
    public class DirectionalMovementSystem : Indicator
    {
        private IndicatorDataSeries _downDmi;

        private MovingAverage _downDmiMa;
        private TrueRange _trueRange;
        private MovingAverage _trueRangeAverage;
        private IndicatorDataSeries _upDmi;
        private MovingAverage _upDmiMa;
        private IndicatorDataSeries _dx;
        private MovingAverage _averageDx;

        [Output("ADX", LineColor = "Turquoise")]
        public IndicatorDataSeries ADX { get; set; }

        [Output("DI+", LineColor = "Green")]
        public IndicatorDataSeries DIPlus { get; set; }

        [Output("DI-", LineColor = "Red")]
        public IndicatorDataSeries DIMinus { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1, MaxValue = 10000)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.WilderSmoothing)]
        public MovingAverageType MAType { get; set; }

        protected override void Initialize()
        {
            _trueRange = Indicators.TrueRange();
            _trueRangeAverage = Indicators.MovingAverage(_trueRange.Result, Periods, MAType);

            _upDmi = CreateDataSeries();
            _downDmi = CreateDataSeries();

            _upDmiMa = Indicators.MovingAverage(_upDmi, Periods, MAType);
            _downDmiMa = Indicators.MovingAverage(_downDmi, Periods, MAType);

            _dx = CreateDataSeries();
            _averageDx = Indicators.MovingAverage(_dx, Periods, MAType);
        }

        public override void Calculate(int index)
        {
            var high = Bars.HighPrices[index];
            var previousHigh = Bars.HighPrices[index - 1];

            var low = Bars.LowPrices[index];
            var previousLow = Bars.LowPrices[index - 1];

            var upMove = high - previousHigh;
            var downMove = previousLow - low;

            if (upMove > downMove && upMove > 0)
                _upDmi[index] = upMove;
            else
                _upDmi[index] = 0;

            if (downMove > upMove && downMove > 0)
                _downDmi[index] = downMove;
            else
                _downDmi[index] = 0;

            var trueRange = _trueRangeAverage.Result[index];

            var diPlus = 100 * _upDmiMa.Result[index] / trueRange;
            var diMinus = 100 * _downDmiMa.Result[index] / trueRange;

            DIPlus[index] = diPlus;
            DIMinus[index] = diMinus;

            var absDifference = Math.Abs(diPlus - diMinus);
            var diSum = diPlus + diMinus;

            _dx[index] = diSum switch
            {
                > 0 => 100 * absDifference / diSum,
                double.NaN => double.NaN,
                _ => 0,
            };

            ADX[index] = _averageDx.Result[index];
        }
    }
}