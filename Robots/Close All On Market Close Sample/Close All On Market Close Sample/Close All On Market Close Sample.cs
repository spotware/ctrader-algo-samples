// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot automatically closes all open positions shortly before the market closes, based on
//    the specified time buffer.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Globalization;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CloseAllOnMarketCloseSample : Robot
    {
        private TimeSpan _closeBeforeTime;  // Store the parsed time span to close positions before market close.

        [Parameter("Close Before", DefaultValue = "00:05:00")]
        public string CloseBeforeTime { get; set; } // Buffer time before market close for closing positions.

        // This method is called when the bot starts.
        protected override void OnStart()
        {
            // Parses the CloseBeforeTime parameter; stops the bot if parsing fails.
            if (!TimeSpan.TryParse(CloseBeforeTime, CultureInfo.InvariantCulture, out _closeBeforeTime))
            {
                Print("You have provided invalid value for Close Before parameter");

                Stop();
            }

            Timer.Start(1); // Starts a timer to check every second for the market closing time.
        }

        // This method is triggered by the Timer at regular intervals.
        protected override void OnTimer()
        {
            var timeTillClose = Symbol.MarketHours.TimeTillClose();  // Retrieves time left until market close.

            // If the market is closed or the time till close is greater than the buffer, exit the method.
            if (!Symbol.MarketHours.IsOpened() || timeTillClose > _closeBeforeTime) return;

            // Loops through all open positions and closes each one.
            foreach (var position in Positions)
            {
                ClosePosition(position);  // Closes the position.
            }
        }
    }
}
