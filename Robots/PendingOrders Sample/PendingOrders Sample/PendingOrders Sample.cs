// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot listens for events such as creation, modification, filling and cancellation of pending 
//    orders and logs the related information.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrdersSample : Robot
    {
        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Subscribes to various pending orders events: Cancelled, Created, Modified, Filled.
            PendingOrders.Cancelled += PendingOrders_Cancelled;
            PendingOrders.Created += PendingOrders_Created;
            PendingOrders.Modified += PendingOrders_Modified;
            PendingOrders.Filled += PendingOrders_Filled;

            // LINQ query to filter orders with MyOrders label.
            var myOrders = PendingOrders.Where(order => order.Label.Equals("MyOrders", StringComparison.OrdinalIgnoreCase));

            // LINQ query to select target prices of all pending orders.
            var orderPrices = from order in PendingOrders
                              select order.TargetPrice;

            // LINQ query to select symbols of all pending orders.
            var orderSymbols = from order in PendingOrders
                               let symbol = Symbols.GetSymbol(order.SymbolName)  // Retrieve the symbol for each order.
                               select symbol;
        }

        private void PendingOrders_Filled(PendingOrderFilledEventArgs obj)  // Handle the event when a pending order is filled.
        {
            Print("Order Filled: {0}", obj.PendingOrder.Id);  // Print the ID of the filled order.
        }

        private void PendingOrders_Modified(PendingOrderModifiedEventArgs obj)  // Handle the event when a pending order is modified.
        {
            Print("Order Modified: {0}", obj.PendingOrder.Id);  // Print the ID of the modified order.
        }

        private void PendingOrders_Created(PendingOrderCreatedEventArgs obj)  // Handle the event when a pending order is created.
        {
            Print("Order Created: {0}", obj.PendingOrder.Id);  // Print the ID of the created order.
        }

        private void PendingOrders_Cancelled(PendingOrderCancelledEventArgs obj)  // Handle the event when a pending order is cancelled.
        {
            Print("Order Cancelled: {0}", obj.PendingOrder.Id);  // Print the ID of the cancelled order.
        }
    }
}
