// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to use the StopTriggerMethod in various trading operations. It sets
//    StopTriggerMethod for market orders, stop orders, and modifies existing positions and pending
//    orders based on the specified StopTriggerMethod parameter.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class StopTriggerMethodSample : Robot
    {
        // Define input parameter for selecting the StopTriggerMethod.
        [Parameter("Stop Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopTriggerMethod { get; set; }  // Set the StopTriggerMethod for orders and modifications.

        // This method is called when the cBot starts and handles the StopTriggerMethod setup.
        protected override void OnStart()
        {
            // Open a new market order with the specified StopTriggerMethod.
            ExecuteMarketOrder(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin, "StopTriggerMethod Test", 10, 10, string.Empty, false, StopTriggerMethod);

            var target = Symbol.Bid + (100 * Symbol.PipSize);  // Calculate the target price.

            // Place a stop order with the StopTriggerMethod applied to the order.
            PlaceStopOrder(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin, target, "StopTriggerMethod Test", 10, 10, null, string.Empty, false, StopTriggerMethod, StopTriggerMethod);

            // Modify open positions to apply the specified StopTriggerMethod.
            foreach (var position in Positions)
            {
                // Skip positions without a stop loss set.
                if (!position.StopLoss.HasValue) continue;

                ModifyPosition(position, position.StopLoss, position.TakeProfit, position.HasTrailingStop, StopTriggerMethod);  // Update the position StopTriggerMethod.
            }

            // Modify open pending orders (Stop and StopLimit) to apply the specified StopTriggerMethod.
            foreach (var order in PendingOrders)
            {
                // Skip orders without a stop loss or orders that are not Stop or StopLimit.
                if (!order.StopLossPips.HasValue || order.OrderType == PendingOrderType.Limit) continue;

                ModifyPendingOrder(order, order.TargetPrice, order.StopLossPips, order.TakeProfitPips, order.ExpirationTime, order.VolumeInUnits, order.HasTrailingStop, StopTriggerMethod, StopTriggerMethod);  // Apply StopTriggerMethod for the order.
            }
        }
    }
}
