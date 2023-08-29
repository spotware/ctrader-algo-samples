// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Automate API example.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AutoRescale = false, AccessRights = AccessRights.None)]
    public class SampleReferenceSMA : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14)]
        public int SmaPeriod { get; set; }

        [Output("Referenced SMA Output")]
        public IndicatorDataSeries refSMA { get; set; }

        private SampleSMA sma;

        protected override void Initialize()
        {
            sma = Indicators.GetIndicator<SampleSMA>(Source, SmaPeriod);
        }

        public override void Calculate(int index)
        {
            refSMA[index] = sma.Result[index];
        }
    }
}
