using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class CenterOfGravity : Indicator
    {
        private IndicatorDataSeries _medianPriceDataSeries;

        [Output("CoG", LineColor = "Red")]
        public IndicatorDataSeries Result { get; set; }

        [Output("Lag", LineColor = "Blue")]
        public IndicatorDataSeries Lag { get; set; }

        [Parameter(DefaultValue = 10, MinValue = 1, MaxValue = 2000)]
        public int Length { get; set; }

        protected override void Initialize()
        {
            _medianPriceDataSeries = Indicators.MedianPrice().Result;
        }

        public override void Calculate(int index)
        {
            if (index < Length + 1)
                return;

            var numerator = 0d;
            var denominator = 0d;
            for (var i = 0; i < Length; i++)
            {
                numerator += (1 + i) * _medianPriceDataSeries[index - i];
                denominator += _medianPriceDataSeries[index - i];
            }

            if (denominator != 0)
                Result[index] = -numerator / denominator + (Length + 1) / 2d;

            Lag[index] = Result[index - 1];
        }
    }
}