// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot example calculates the number of bars that have passed since each position with a 
//    specified label was opened. The bot demonstrates how to work with position properties and 
//    bar indexing in the cTrader API. 
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionBarsPassedSample : Robot
    {
        [Parameter("Label", DefaultValue = "MyBot")]
        public string Label { get; set; }  // Store the unique label for filtering positions.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            var currentIndex = Bars.Count - 1;  // Get the index of the most recent bar.

            // Loop through all open positions.
            foreach (var position in Positions)
            {
                // Check if the position label matches the specified label.
                if (!string.Equals(position.Label, Label, StringComparison.OrdinalIgnoreCase)) continue;

                // Determine the index of the bar when the position was opened.
                var positionOpenBarIndex = Bars.OpenTimes.GetIndexByTime(position.EntryTime);

                // Calculate the number of bars that have passed since the position was opened.
                var numberOfBarsPassedSincePositionOpened = currentIndex - positionOpenBarIndex;

                // Output the calculated number of bars to the log.
                Print(numberOfBarsPassedSincePositionOpened);
            }
        }
    }
}
