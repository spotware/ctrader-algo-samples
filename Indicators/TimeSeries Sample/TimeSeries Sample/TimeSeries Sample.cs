// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TimeSeriesSample : Indicator
    {
        protected override void Initialize()
        {
            // Every Bars object has a time series which is the open times of bars
            var timeSeries = Bars.OpenTimes;

            Print("Count: ", timeSeries.Count);

            // You can get another bars index by using TimeSeries GetIndexByTime/GetIndexByExactTime methods

            var dailyBars = MarketData.GetBars(TimeFrame.Daily);

            var dailyBarsIndex = timeSeries.GetIndexByTime(dailyBars.OpenTimes.LastValue);

            var open = Bars.OpenPrices[dailyBarsIndex];

            Print("Daily Bars Index: ", dailyBarsIndex, " | Open: ", open);
        }

        public override void Calculate(int index)
        {
        }
    }
}
