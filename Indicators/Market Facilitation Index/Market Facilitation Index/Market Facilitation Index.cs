using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class MarketFacilitationIndex : Indicator
    {
        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Blue", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            Result[index + Shift] = Bars.TickVolumes[index] == 0 ? 0 : (Bars.HighPrices[index] - Bars.LowPrices[index]) / Bars.TickVolumes[index];
        }
    }
}