// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to handle position closing events. It listens for position closures 
//    and evaluates the reason for the closure, allowing specific actions to be executed based on 
//    the type of close event.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionCloseReasonSample : Robot
    {
        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            Positions.Closed += Positions_Closed;  // Subscribe to the event triggered when a position is closed.
        }

        // This method is triggered whenever a position is closed.
        private void Positions_Closed(PositionClosedEventArgs obj)
        {
            Print(obj.Reason);  // Outputs the reason for the position closure to the log.

            switch (obj.Reason)  // Evaluates the type of closure reason.
            {
                case PositionCloseReason.Closed:
                    // Do something if position closed.
                    break;

                case PositionCloseReason.StopLoss:
                    // Do something if position stop loss got hit.
                    break;

                case PositionCloseReason.StopOut:
                    // Do something if position stopped out.
                    break;

                case PositionCloseReason.TakeProfit:
                    // Do something if position take profit got hit.
                    break;
            }
        }
    }
}
