// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
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