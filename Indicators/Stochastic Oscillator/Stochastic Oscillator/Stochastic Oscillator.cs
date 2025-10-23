using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None, IsPercentage = true)]
    public class StochasticOscillator : Indicator
    {
        private IndicatorDataSeries _fastK;
        private MovingAverage _slowK;
        private MovingAverage _averageOnSlowK;

        [Parameter("%K Periods", DefaultValue = 9, MinValue = 1)]
        public int KPeriods { get; set; }

        [Parameter("%K Slowing", DefaultValue = 3, MinValue = 1)]
        public int KSlowing { get; set; }

        [Parameter("%D Periods", DefaultValue = 9, MinValue = 0)]
        public int DPeriods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MAType { get; set; }

        [Parameter("Calculation Type", DefaultValue = StochasticCalculationType.LowHigh)]
        public StochasticCalculationType CalculationType { get; set; }

        [Output("%D", LineColor = "Red", LineStyle = LineStyle.Lines)]
        public IndicatorDataSeries PercentD { get; set; }

        [Output("%K", LineColor = "Green")]
        public IndicatorDataSeries PercentK { get; set; }

        protected override void Initialize()
        {
            _fastK = CreateDataSeries();
            _slowK = Indicators.MovingAverage(_fastK, KSlowing, MAType);

            _averageOnSlowK = Indicators.MovingAverage(_slowK.Result, DPeriods, MAType);
        }

        public override void Calculate(int index)
        {
            _fastK[index] = GetFastKValue(index);

            PercentK[index] = _slowK.Result[index];
            PercentD[index] = _averageOnSlowK.Result[index];
        }

        private double GetFastKValue(int index)
        {
            var lowestLow = GetMinFromPeriod(
                CalculationType == StochasticCalculationType.LowHigh ? Bars.LowPrices : Bars.ClosePrices,
                index,
                KPeriods);

            var hightestHigh = GetMaxFromPeriod(
                CalculationType == StochasticCalculationType.LowHigh ? Bars.HighPrices : Bars.ClosePrices,
                index,
                KPeriods);
            var currentClose = Bars.ClosePrices[index];

            return (currentClose - lowestLow) / (hightestHigh - lowestLow) * 100;
        }

        private static double GetMinFromPeriod(DataSeries dataSeries, int endIndex, int periods)
        {
            var min = dataSeries[endIndex];
            for (var i = endIndex; i > endIndex - periods; i--)
                if (dataSeries[i] < min)
                    min = dataSeries[i];

            return min;
        }

        private static double GetMaxFromPeriod(DataSeries dataSeries, int endIndex, int periods)
        {
            var max = dataSeries[endIndex];
            for (var i = endIndex; i > endIndex - periods; i--)
                if (dataSeries[i] > max)
                    max = dataSeries[i];

            return max;
        }
    }
}