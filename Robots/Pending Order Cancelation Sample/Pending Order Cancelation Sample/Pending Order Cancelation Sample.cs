// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot searches for a pending order based on the specified comment and label, and if found,
//    cancels the order. It allows filtering the orders by comment, label or both.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderCancelationSample : Robot
    {
        [Parameter("Order Comment")]
        public string OrderComment { get; set; }  // Store the comment for the order.

        [Parameter("Order Label")]
        public string OrderLabel { get; set; }  // Store the label for the order.

        // This method is called when the bot starts.
        protected override void OnStart()
        {
            PendingOrder order = null;  // Initialize a variable to store the pending order.

            if (!string.IsNullOrWhiteSpace(OrderComment) && !string.IsNullOrWhiteSpace(OrderLabel))  // Checks if both order comment and label are provided.
            {
                // Searches for the pending order with the specified comment and label.
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase) && string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(OrderComment))  // If only the comment is provided.
            {
                // Searches for the pending order with the specified comment.
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(OrderLabel))  // If only the label is provided.
            {
                // Searches for the pending order with the specified label.
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }
            if (order == null)  // If no order matching the criteria was found.
            {
                Print("Couldn't find the order, please check the comment and label");  // Prints an error message.

                Stop();  // Stops the robot as the order could not be found.
            }

            CancelPendingOrder(order);  // Cancels the found pending order.
        }
    }
}
