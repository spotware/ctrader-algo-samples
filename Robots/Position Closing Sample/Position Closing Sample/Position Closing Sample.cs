// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to find and close a trading position based on its label, comment 
//    or a combination of both. It identifies the relevant position and executes the closing process.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionClosingSample : Robot
    {
        [Parameter("Position Comment")]
        public string PositionComment { get; set; }  // Parameter to specify the comment associated with a position.

        [Parameter("Position Label")]
        public string PositionLabel { get; set; }  // Parameter to specify the label associated with a position.

        // The main method executed when the cBot starts.
        protected override void OnStart()
        {
            Position position = null;  // Initialize a variable to hold the target position.

            // Check if both comment and label parameters are provided.
            if (!string.IsNullOrWhiteSpace(PositionComment) && !string.IsNullOrWhiteSpace(PositionLabel))
            {
                // Find the first position that matches the label and comment.
                position = Positions.FindAll(PositionLabel).FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            
            // Check if only the comment parameter is provided.
            else if (!string.IsNullOrWhiteSpace(PositionComment))
            {
                // Find the first position that matches the comment.
                position = Positions.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            
            // Check if only the label parameter is provided.
            else if (!string.IsNullOrWhiteSpace(PositionLabel))
            {
                // Find the first position that matches the label.
                position = Positions.Find(PositionLabel);
            }

            // If no matching position is found.
            if (position == null)
            {
                Print("Couldn't find the position, please check the comment and label");  // Log a message indicating the position could not be found.
                Stop();  // Stop the cBot to avoid further processing.
            }

            ClosePosition(position);  // Close the identified position.
        }
    }
}
