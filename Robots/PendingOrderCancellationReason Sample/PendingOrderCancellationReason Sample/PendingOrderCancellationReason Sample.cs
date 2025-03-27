// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot listens for the cancellation of pending orders and handles different reasons for
//    cancellation (Cancelled, Expired or Rejected). It prints the cancellation reason and allows 
//    you to add custom actions based on the specific reason for the cancellation. 
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderCancellationReasonSample : Robot
    {
        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            PendingOrders.Cancelled += PendingOrders_Cancelled;  // Subscribe to the event that is triggered when a pending order is cancelled.
        }

        // This method handles the cancellation event.
        private void PendingOrders_Cancelled(PendingOrderCancelledEventArgs obj)
        {
            Print(obj.Reason);  // Print the reason for order cancellation.

            switch (obj.Reason)  // Switches based on the reason for the cancellation.
            {
                case PendingOrderCancellationReason.Cancelled:
                    // Do something if order cancelled.
                    break;

                case PendingOrderCancellationReason.Expired:
                    // Do something if order expired.
                    break;

                case PendingOrderCancellationReason.Rejected:
                    // Do something if order rejected.
                    break;
            }
        }
    }
}
