// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to handle position modification events. 
//    It listens for changes in position details and logs the modified position ID. 
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionModifiedEventArgsSample : Robot
    {
        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Register a handler for the Positions.Modified event to respond when a position is modified.
            Positions.Modified += Positions_Modified;
        }

        // This method is triggered whenever a position is modified.
        private void Positions_Modified(PositionModifiedEventArgs obj)
        {
            // Log the ID of the modified position for monitoring purposes.
            Print("Position {0} modified", obj.Position.Id);
        }
    }
}
