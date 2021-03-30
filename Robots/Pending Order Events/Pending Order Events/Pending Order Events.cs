using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to use PendingOrders events
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderEvents : Robot
    {
        protected override void OnStart()
        {
            PendingOrders.Cancelled += PendingOrders_Cancelled;
            PendingOrders.Modified += PendingOrders_Modified;
            PendingOrders.Filled += PendingOrders_Filled;
        }

        private void PendingOrders_Filled(PendingOrderFilledEventArgs obj)
        {
            var pendingOrderThatFilled = obj.PendingOrder;

            var filledPosition = obj.Position;
        }

        private void PendingOrders_Modified(PendingOrderModifiedEventArgs obj)
        {
            var modifiedOrder = obj.PendingOrder;
        }

        private void PendingOrders_Cancelled(PendingOrderCancelledEventArgs obj)
        {
            var cancelledOrder = obj.PendingOrder;

            var cancellationReason = obj.Reason;
        }
    }
}