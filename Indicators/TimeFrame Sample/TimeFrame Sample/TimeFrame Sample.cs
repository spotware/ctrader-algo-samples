// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TimeFrameSample : Indicator
    {
        [Parameter("Time Frame", DefaultValue = "Daily")]
        public TimeFrame UserSelectedTimeFrame { get; set; }

        protected override void Initialize()
        {
            Print("Name: ", UserSelectedTimeFrame.Name, " | Short Name: ", UserSelectedTimeFrame.ShortName);

            // Getting another time frame bars data, using user selected time frame
            var barsBasedOnUserSelectedTimeFrame = MarketData.GetBars(UserSelectedTimeFrame);
            // Getting another time frame bars data, using pre-defined TimeFrames
            var barsBasedOnOtherTimeFrame = MarketData.GetBars(TimeFrame.Day2);
        }

        public override void Calculate(int index)
        {
        }
    }
}
