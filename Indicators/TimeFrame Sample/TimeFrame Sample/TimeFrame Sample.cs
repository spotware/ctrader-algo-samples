using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to get a time frame from user via parameters and the get that time frame bars
    /// Also you can use the pre-defiend time frames
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TimeFrameSample : Indicator
    {
        // Getting time frame via a parameter from user
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