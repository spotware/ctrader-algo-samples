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
    public class StopTriggerMethodSample : Robot
    {
        [Parameter("Stop Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopTriggerMethod { get; set; }

        protected override void OnStart()
        {
            // Setting a new position StopTriggerMethod
            ExecuteMarketOrder(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin, "StopTriggerMethod Test", 10, 10, string.Empty, false, StopTriggerMethod);

            // Setting a new stop order StopTriggerMethod for both order and its stop loss
            var target = Symbol.Bid + (100 * Symbol.PipSize);

            PlaceStopOrder(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin, target, "StopTriggerMethod Test", 10, 10, null, string.Empty, false, StopTriggerMethod, StopTriggerMethod);

            // Changing open positions StopTriggerMethod
            foreach (var position in Positions)
            {
                if (!position.StopLoss.HasValue) continue;

                ModifyPosition(position, position.StopLoss, position.TakeProfit, position.HasTrailingStop, StopTriggerMethod);
            }

            // Changing open pending orders (Stop and StopLimit) StopTriggerMethod
            foreach (var order in PendingOrders)
            {
                if (!order.StopLossPips.HasValue || order.OrderType == PendingOrderType.Limit) continue;

                ModifyPendingOrder(order, order.TargetPrice, order.StopLossPips, order.TakeProfitPips, order.ExpirationTime, order.VolumeInUnits, order.HasTrailingStop, StopTriggerMethod, StopTriggerMethod);
            }
        }
    }
}