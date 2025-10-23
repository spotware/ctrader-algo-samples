using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class AccumulationDistribution : Indicator
    {
        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var previousValue = double.IsNaN(Result[index - 1]) ? 0 : Result[index - 1];

            var close = Bars.ClosePrices[index];
            var low = Bars.LowPrices[index];
            var high = Bars.HighPrices[index];
            var volume = Bars.TickVolumes[index];

            if (high - low == 0)
            {
                Result[index] = previousValue;
                return;
            }

            var closeLocationValue = (close - low - (high - close)) / (high - low);
            var moneyFlowVolume = closeLocationValue * volume;
            Result[index] = previousValue + moneyFlowVolume;
        }
    }
}