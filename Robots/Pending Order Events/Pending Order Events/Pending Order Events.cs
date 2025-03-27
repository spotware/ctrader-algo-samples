// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot is intended to demonstrate the handling of pending order events. It subscribes to 
//    three events related to pending orders: Cancelled, Modified and Filled. The cBot performs 
//    actions when these events occur, such as processing the filled order or handling modifications 
//    and cancellations.
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderEvents : Robot
    {
        // This method is called when the bot starts.
        protected override void OnStart()
        {
            PendingOrders.Cancelled += PendingOrders_Cancelled;  // Subscribe to the PendingOrders Cancelled event.
            PendingOrders.Modified += PendingOrders_Modified;  // Subscribe to the PendingOrders Modified event.
            PendingOrders.Filled += PendingOrders_Filled;  // Subscribe to the PendingOrders Filled event.
        }

        // This method is triggered when a pending order is filled.
        private void PendingOrders_Filled(PendingOrderFilledEventArgs obj)
        {
            var pendingOrderThatFilled = obj.PendingOrder;  // Retrieve the pending order that was filled.

            var filledPosition = obj.Position;  // Retrieve the position associated with the filled order.
        }

        // This method is triggered when a pending order is modified.
        private void PendingOrders_Modified(PendingOrderModifiedEventArgs obj)
        {
            var modifiedOrder = obj.PendingOrder;  // Retrieve the modified pending order.
        }

        // This method is triggered when a pending order is cancelled.
        private void PendingOrders_Cancelled(PendingOrderCancelledEventArgs obj)
        {
            var cancelledOrder = obj.PendingOrder;  // Retrieve the cancelled pending order.

            var cancellationReason = obj.Reason;  // Retrieve the reason for the cancellation.
        }
    }
}
