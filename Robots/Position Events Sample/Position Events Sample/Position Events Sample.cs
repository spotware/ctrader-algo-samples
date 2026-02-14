// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot tracks position events, including opening, closing and modifications.
//    It subscribes to position-related events and processes data when these events occur.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionEventsSample : Robot
    {
        // The main method executed when the cBot starts.
        protected override void OnStart()
        {
            Positions.Opened += Positions_Opened;  // Subscribe to the Positions.Opened event to handle actions when a position is opened.
            Positions.Closed += Positions_Closed;  // Subscribe to the Positions.Closed event to handle actions when a position is closed.
            Positions.Modified += Positions_Modified;  // Subscribe to the Positions.Modified event to handle actions when a position is modified.
        }

        // This method is triggered whenever an existing position is modified.
        private void Positions_Modified(PositionModifiedEventArgs obj)
        {
            // Capture details of the modified position for further processing or logging.
            var modifiedPosition = obj.Position;
        }

        // This method is triggered whenever a position is closed.
        private void Positions_Closed(PositionClosedEventArgs obj)
        {
            // Capture details of the closed position, including the position itself and the reason for closing.
            var closedPosition = obj.Position;

            // Identify the reason for the position closure (e.g., StopLoss, TakeProfit or ManualClose).
            var closeReason = obj.Reason;
        }

        // This method is triggered whenever a position is opened.
        private void Positions_Opened(PositionOpenedEventArgs obj)
        {
            // Capture details of the newly opened position for further processing or logging.
            var openedPosition = obj.Position;
        }
    }
}
