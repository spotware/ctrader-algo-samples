using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 0, AccessRights = AccessRights.None)]
    public class VolumeROC : Indicator
    {
        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            if (index < Periods)
                return;

            var volumePeriodsAgo = Bars.TickVolumes[index - Periods];
            var volume = Bars.TickVolumes[index];
            Result[index + Shift] = (volume - volumePeriodsAgo) * 100d / volumePeriodsAgo;
        }
    }
}