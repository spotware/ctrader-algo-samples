using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class ChaikinMoneyFlow : Indicator
    {
        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            if (index < Periods)
                return;

            double moneyFlowVolumeSum = 0;
            double volumeSum = 0;

            for (var i = 0; i < Periods; i++)
            {
                var close = Bars.ClosePrices[index - i];
                var low = Bars.LowPrices[index - i];
                var high = Bars.HighPrices[index - i];
                var volume = Bars.TickVolumes[index - i];

                if (high - low != 0)
                {
                    var moneyFlowMultiplier = (close - low - (high - close)) / (high - low);
                    var moneyFlowVolume = moneyFlowMultiplier * volume;
                    moneyFlowVolumeSum += moneyFlowVolume;
                }

                volumeSum += volume;
            }

            Result[index] = moneyFlowVolumeSum / volumeSum;
        }
    }
}