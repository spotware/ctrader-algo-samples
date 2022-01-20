using System;
using System.Linq;
using cAlgo.API;

namespace cAlgo.Robots
{
    // This sample shows how to use the PendingOrders collection
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrdersSample : Robot
    {
        protected override void OnStart()
        {
            PendingOrders.Cancelled += PendingOrders_Cancelled;
            PendingOrders.Created += PendingOrders_Created;
            PendingOrders.Modified += PendingOrders_Modified;
            PendingOrders.Filled += PendingOrders_Filled;

            // You can use Linq to execute queries over all open orders
            var myOrders = PendingOrders.Where(order => order.Label.Equals("MyOrders", StringComparison.OrdinalIgnoreCase));

            var orderPrices = from order in PendingOrders
                              select order.TargetPrice;

            var orderSymbols = from order in PendingOrders
                               let symbol = Symbols.GetSymbol(order.SymbolName)
                               select symbol;
        }

        private void PendingOrders_Filled(PendingOrderFilledEventArgs obj)
        {
            Print("Order Filled: {0}", obj.PendingOrder.Id);
        }

        private void PendingOrders_Modified(PendingOrderModifiedEventArgs obj)
        {
            Print("Order Modified: {0}", obj.PendingOrder.Id);
        }

        private void PendingOrders_Created(PendingOrderCreatedEventArgs obj)
        {
            Print("Order Created: {0}", obj.PendingOrder.Id);
        }

        private void PendingOrders_Cancelled(PendingOrderCancelledEventArgs obj)
        {
            Print("Order Cancelled: {0}", obj.PendingOrder.Id);
        }
    }
}