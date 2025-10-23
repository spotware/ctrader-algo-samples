using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class IchimokuKinkoHyo : Indicator
    {
        public const string SenkouSpanAName = "Senkou Span A";
        public const string SenkouSpanBName = "Senkou Span B";

        [Parameter("Tenkan Sen Periods", DefaultValue = 9, MinValue = 1)]
        public int TenkanSenPeriods { get; set; }

        [Parameter("Kijun Sen Periods", DefaultValue = 26, MinValue = 1)]
        public int KijunSenPeriods { get; set; }

        [Parameter("Senkou Span B Periods", DefaultValue = 52, MinValue = 1)]
        public int SenkouSpanBPeriods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -200, MaxValue = 200)]
        public int Shift { get; set; }

        [Output("Tenkan Sen", LineColor = "DodgerBlue")]
        public IndicatorDataSeries TenkanSen { get; set; }

        [Output("Kijun Sen", LineColor = "Crimson")]
        public IndicatorDataSeries KijunSen { get; set; }

        [Output("Chikou Span", LineColor = "MediumSpringGreen")]
        public IndicatorDataSeries ChikouSpan { get; set; }

        [Output(SenkouSpanAName, LineColor = "SeaGreen")]
        public IndicatorDataSeries SenkouSpanA { get; set; }

        [Output(SenkouSpanBName, LineColor = "Red")]
        public IndicatorDataSeries SenkouSpanB { get; set; }

        public override void Calculate(int index)
        {
            if (index < TenkanSenPeriods || index < SenkouSpanBPeriods)
                return;

            var maxFast = Bars.HighPrices[index];
            var minFast = Bars.LowPrices[index];
            var maxMedium = Bars.HighPrices[index];
            var minMedium = Bars.LowPrices[index];
            var maxSlow = Bars.HighPrices[index];
            var minSlow = Bars.LowPrices[index];

            for (var i = 0; i < TenkanSenPeriods; i++)
            {
                if (maxFast < Bars.HighPrices[index - i])
                    maxFast = Bars.HighPrices[index - i];

                if (minFast > Bars.LowPrices[index - i])
                    minFast = Bars.LowPrices[index - i];
            }

            for (var i = 0; i < KijunSenPeriods; i++)
            {
                if (maxMedium < Bars.HighPrices[index - i])
                    maxMedium = Bars.HighPrices[index - i];

                if (minMedium > Bars.LowPrices[index - i])
                    minMedium = Bars.LowPrices[index - i];
            }

            for (var i = 0; i < SenkouSpanBPeriods; i++)
            {
                if (maxSlow < Bars.HighPrices[index - i])
                    maxSlow = Bars.HighPrices[index - i];

                if (minSlow > Bars.LowPrices[index - i])
                    minSlow = Bars.LowPrices[index - i];
            }

            TenkanSen[index + Shift] = (maxFast + minFast) / 2;
            KijunSen[index + Shift] = (maxMedium + minMedium) / 2;

            ChikouSpan[index - KijunSenPeriods + Shift] = Bars.ClosePrices[index];

            SenkouSpanA[index + KijunSenPeriods + Shift] = (TenkanSen[index + Shift] + KijunSen[index + Shift]) / 2;
            SenkouSpanB[index + KijunSenPeriods + Shift] = (maxSlow + minSlow) / 2;
        }
    }
}