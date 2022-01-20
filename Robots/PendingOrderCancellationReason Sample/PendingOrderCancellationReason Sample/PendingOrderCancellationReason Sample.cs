using cAlgo.API;

namespace cAlgo.Robots
{
    // This sample shows how to use PendingOrderCancellationReason
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderCancellationReasonSample : Robot
    {
        protected override void OnStart()
        {
            PendingOrders.Cancelled += PendingOrders_Cancelled;
        }

        private void PendingOrders_Cancelled(PendingOrderCancelledEventArgs obj)
        {
            Print(obj.Reason);

            switch (obj.Reason)
            {
                case PendingOrderCancellationReason.Cancelled:
                    // Do something if order cancelled
                    break;

                case PendingOrderCancellationReason.Expired:
                    // Do something if order expired
                    break;

                case PendingOrderCancellationReason.Rejected:
                    // Do something if order rejected
                    break;
            }
        }
    }
}