// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to modify pending orders in cAlgo. The bot allows modifications to
//    pending orders based on parameters such as Target Price, Stop Loss, Take Profit, Expiry Time 
//    and more. It supports modifying Limit, Stop and Stop Limit orders.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Globalization;
using System.Linq;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderModificationSample : Robot
    {
        // Define input parameters for the cBot.
        [Parameter("Order Comment")]
        public string OrderComment { get; set; }  // Order comment for the pending order.

        [Parameter("Order Label")]
        public string OrderLabel { get; set; }  // Order label for the pending order.

        [Parameter("Target Price", DefaultValue = 0.0)]
        public double TargetPrice { get; set; }  // Target price for the pending order, which defaults to 0.0.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Stop Loss Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; } // Stop loss trigger method, default value is Trade.

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Expiry (HH:mm:ss)")]
        public string Expiry { get; set; }  // Expiry time for the pending order in HH:mm:ss format.

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Volume in lots, which defaults to 0.01.

        [Parameter("Has Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }  // Whether to use a trailing stop, default value is false.

        [Parameter("Order Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod OrderTriggerMethod { get; set; }  // Order trigger method for stop orders, default value is Trade.

        [Parameter("Limit Range (Pips)", DefaultValue = 10)]
        public double LimitRangeInPips { get; set; }  // Limit range in pips for stop limit orders, which defaults to 10.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            PendingOrder order = null;

            // Search for a pending order using both comment and label.
            // If found, the order is stored in the variable 'order' for further modifications.
            if (!string.IsNullOrWhiteSpace(OrderComment) && !string.IsNullOrWhiteSpace(OrderComment))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase) && string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }
            
            // Search for a pending order using comment only.
            // If found, the order is stored in the variable 'order' for further modifications.
            else if (!string.IsNullOrWhiteSpace(OrderComment))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase));
            }
            
            // Search for a pending order using label only.
            // If found, the order is stored in the variable 'order' for further modifications.
            else if (!string.IsNullOrWhiteSpace(OrderLabel))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }

            // Stops execution if no matching order is found.
            if (order == null)
            {
                Print("Couldn't find the order, please check the comment and label");
                Stop();
            }

            // Set target price or keep the original value.
            var targetPrice = TargetPrice == 0 ? order.TargetPrice : TargetPrice;

            // Get symbol details of the order.
            var orderSymbol = Symbols.GetSymbol(order.SymbolName);

            // Set stop-loss and take-profit values or keep the original.
            var stopLossInPips = StopLossInPips == 0 ? order.StopLossPips : (double?)StopLossInPips;
            var takeProfitInPips = TakeProfitInPips == 0 ? order.TakeProfitPips : (double?)TakeProfitInPips;

            DateTime? expiryTime;  // Declare a nullable DateTime variable to store the expiry time of the pending order.

            // Set expiry time or keep the original value.
            if (string.IsNullOrWhiteSpace(Expiry))
            {
                expiryTime = order.ExpirationTime;
            }
            
            // Set expiry to null if explicitly defined as "0".
            else if (Expiry.Equals("0", StringComparison.OrdinalIgnoreCase))
            {
                expiryTime = null;
            }
            
             // Parse expiry time from input.
            else
            {
                var expiryTimeSpan = default(TimeSpan);  // Initialize a TimeSpan variable to hold the parsed expiry duration.

                 // Try to parse the "Expiry" parameter into a TimeSpan using HH:mm:ss format.
                if (!TimeSpan.TryParse(Expiry, CultureInfo.InvariantCulture, out expiryTimeSpan))
                {
                     // If parsing fails, print an error message and stop the cBot execution.
                    Print("Your provided value for expiry is not valid, please use HH:mm:ss format");
                    Stop();
                }

                 // If the parsed TimeSpan is valid and not the default, calculate expiry time.
                expiryTime = expiryTimeSpan == default(TimeSpan) ? null : (DateTime?)Server.Time.Add(expiryTimeSpan);
            }

            // Calculate volume in units or keep the original.
            var volumeInUnits = VolumeInLots == 0 ? order.VolumeInUnits : orderSymbol.QuantityToVolumeInUnits(VolumeInLots);

            // Check if the pending order is of type Limit.
            // If yes, modify the order with the specified parameters.
            if (order.OrderType == PendingOrderType.Limit)
            {
                ModifyPendingOrder(order, targetPrice, stopLossInPips, takeProfitInPips, expiryTime, volumeInUnits, HasTrailingStop, StopLossTriggerMethod);
            }

            // Check if the pending order is of type Stop.
            // If yes, modify the order with the specified parameters, including the order trigger method.
            else if (order.OrderType == PendingOrderType.Stop)
            {
                ModifyPendingOrder(order, targetPrice, stopLossInPips, takeProfitInPips, expiryTime, volumeInUnits, HasTrailingStop, StopLossTriggerMethod, OrderTriggerMethod);
            }
            
            // Check if the pending order is of type StopLimit.
            // If yes, modify the order with the specified parameters, including the limit range in pips.
            else if (order.OrderType == PendingOrderType.StopLimit)
            {
                ModifyPendingOrder(order, targetPrice, stopLossInPips, takeProfitInPips, expiryTime, volumeInUnits, HasTrailingStop, StopLossTriggerMethod, OrderTriggerMethod, LimitRangeInPips);
            }
        }
    }
}
