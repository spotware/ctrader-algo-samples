// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderCancelationSample : Robot
    {
        [Parameter("Order Comment")]
        public string OrderComment { get; set; }

        [Parameter("Order Label")]
        public string OrderLabel { get; set; }

        protected override void OnStart()
        {
            PendingOrder order = null;

            if (!string.IsNullOrWhiteSpace(OrderComment) && !string.IsNullOrWhiteSpace(OrderLabel))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase) && string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(OrderComment))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(OrderLabel))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }

            if (order == null)
            {
                Print("Couldn't find the order, please check the comment and label");

                Stop();
            }

            CancelPendingOrder(order);
        }
    }
}