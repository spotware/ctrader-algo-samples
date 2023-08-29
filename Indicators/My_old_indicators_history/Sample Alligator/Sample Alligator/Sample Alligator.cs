// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Automate API example.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AutoRescale = false, AccessRights = AccessRights.None)]
    public class SampleAlligator : Indicator
    {
        [Parameter("Periods", Group = "Jaws", DefaultValue = 13)]
        public int JawsPeriods { get; set; }

        [Parameter("Shift", Group = "Jaws", DefaultValue = 8)]
        public int JawsShift { get; set; }

        [Parameter("Periods", Group = "Teeth", DefaultValue = 8)]
        public int TeethPeriods { get; set; }

        [Parameter("Shift", Group = "Teeth", DefaultValue = 5)]
        public int TeethShift { get; set; }

        [Parameter("Periods", Group = "Lips", DefaultValue = 5)]
        public int LipsPeriods { get; set; }

        [Parameter("Shift", Group = "Lips", DefaultValue = 3)]
        public int LipsShift { get; set; }

        [Output("Jaws", LineColor = "Blue")]
        public IndicatorDataSeries Jaws { get; set; }

        [Output("Teeth", LineColor = "Red")]
        public IndicatorDataSeries Teeth { get; set; }

        [Output("Lips", LineColor = "Lime")]
        public IndicatorDataSeries Lips { get; set; }

        private WellesWilderSmoothing jawsMa;
        private WellesWilderSmoothing teethMa;
        private WellesWilderSmoothing lipsMa;

        protected override void Initialize()
        {
            jawsMa = Indicators.WellesWilderSmoothing(Bars.MedianPrices, JawsPeriods);
            teethMa = Indicators.WellesWilderSmoothing(Bars.MedianPrices, TeethPeriods);
            lipsMa = Indicators.WellesWilderSmoothing(Bars.MedianPrices, LipsPeriods);
        }

        public override void Calculate(int index)
        {
            Jaws[index + JawsShift] = jawsMa.Result[index];
            Teeth[index + TeethShift] = teethMa.Result[index];
            Lips[index + LipsShift] = lipsMa.Result[index];
        }
    }
}