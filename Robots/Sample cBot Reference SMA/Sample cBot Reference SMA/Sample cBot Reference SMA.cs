// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SamplecBotReferenceSMA : Robot
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("SMA Period", DefaultValue = 14)]
        public int SmaPeriod { get; set; }

        private SampleSMA sma;

        protected override void OnStart()
        {
            sma = Indicators.GetIndicator<SampleSMA>(Source, SmaPeriod);
        }

        protected override void OnTick()
        {
            Print("{0}", sma.Result.LastValue);
        }
    }
}
