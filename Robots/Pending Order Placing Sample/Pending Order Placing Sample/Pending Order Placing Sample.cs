// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot places pending orders (Limit, Stop or StopLimit) for the specified trade direction, 
//    volume and other parameters. It allows users to set a trailing stop, specify expiry times and 
//    choose whether orders are placed asynchronously or synchronously. The bot validates the parameters, 
//    handles the order placement process and invokes a callback function when the trade is completed.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Globalization;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderPlacingSample : Robot
    {
        [Parameter("Type", DefaultValue = PendingOrderType.Limit)]
        public PendingOrderType OrderType { get; set; }  // Order type parameter with a default value of PendingOrderType.Limit.

        [Parameter("Direction", DefaultValue = TradeType.Buy)]
        public TradeType OrderTradeType { get; set; }  // Trade direction (buy or sell) for the order.

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Volume in lots for the trade, defaulting to 0.01 lots.

        [Parameter("Distance (Pips)", DefaultValue = 20, MinValue = 1)]
        public double DistanceInPips { get; set; }  // Distance in pips from the current price for pending orders, defaulting to 20 pips.

        [Parameter("Stop (Pips)", DefaultValue = 10, MinValue = 0)]
        public double StopInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Target (Pips)", DefaultValue = 10, MinValue = 0)]
        public double TargetInPips { get; set; }  // Target (take profit) in pips, defaulting to 10 pips.

        [Parameter("Limit Range (Pips)", DefaultValue = 10, MinValue = 1)]
        public double LimitRangeInPips { get; set; }  // Limit range for stop limit orders, defaulting to 10 pips.

        [Parameter("Expiry", DefaultValue = "00:00:00")]
        public string Expiry { get; set; }  // Expiry time for pending orders in HH:mm:ss format.

        [Parameter("Label")]
        public string Label { get; set; }  // Label for the order to help identify it.

        [Parameter("Comment")]
        public string Comment { get; set; }  // Comment for the order to provide additional context.

        [Parameter("Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }  // Enable or disable trailing stop for the order, disabled by default.

        [Parameter("Stop Loss Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; }  // Method for stop loss trigger.

        [Parameter("Stop Order Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopOrderTriggerMethod { get; set; }  // Method for stop order trigger.

        [Parameter("Async", DefaultValue = false)]
        public bool IsAsync { get; set; }  // Whether the order should be placed asynchronously, disabled by default.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Convert the specified volume in lots to volume in units for the trading symbol.
            var volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Adjust the distance in pips according to the symbol pip size.
            DistanceInPips *= Symbol.PipSize;

            // Set stop-loss and take-profit values or null if they are zero.
            var stopLoss = StopInPips == 0 ? null : (double?)StopInPips;
            var takeProfit = TargetInPips == 0 ? null : (double?)TargetInPips;

            TimeSpan expiry;  // Declare expiry as a TimeSpan.

            // Try parsing the expiry string into a TimeSpan.
            if (!TimeSpan.TryParse(Expiry, CultureInfo.InvariantCulture, out expiry))
            {
                // Print an error message and stop the cBot if expiry time is invalid.
                Print("Invalid expiry");
                Stop();
            }

            // Calculate the expiry time, adding the expiry TimeSpan to the server time if valid.
            var expiryTime = expiry != TimeSpan.FromSeconds(0) ? (DateTime?)Server.Time.Add(expiry) : null;

            TradeResult result = null;  // Initialize the trade result variable.

            // Switch to handle different order types: Limit, Stop or StopLimit.
            switch (OrderType)
            {
                case PendingOrderType.Limit:
                    var limitPrice = OrderTradeType == TradeType.Buy ? Symbol.Ask - DistanceInPips : Symbol.Ask + DistanceInPips;  // Calculate the price for a limit order based on the trade direction.

                    // If the IsAsync flag is true, place the limit order asynchronously.
                    if (IsAsync)
                        PlaceLimitOrderAsync(OrderTradeType, SymbolName, volumeInUnits, limitPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, OnCompleted);
                    // If the IsAsync flag is false, place the limit order synchronously.
                    else
                        result = PlaceLimitOrder(OrderTradeType, SymbolName, volumeInUnits, limitPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod);

                    break;

                case PendingOrderType.Stop:
                    var stopPrice = OrderTradeType == TradeType.Buy ? Symbol.Ask + DistanceInPips : Symbol.Ask - DistanceInPips;  // Calculate the stop price based on the trade direction.

                    // If the IsAsync flag is true, place the stop order asynchronously.
                    if (IsAsync)
                        PlaceStopOrderAsync(OrderTradeType, SymbolName, volumeInUnits, stopPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod, OnCompleted);
                    // If the IsAsync flag is false, place the stop order synchronously.
                    else
                        result = PlaceStopOrder(OrderTradeType, SymbolName, volumeInUnits, stopPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod);  // Place the stop order.

                    break;

                case PendingOrderType.StopLimit:
                    var stopLimitPrice = OrderTradeType == TradeType.Buy ? Symbol.Ask + DistanceInPips : Symbol.Ask - DistanceInPips;  // Calculate the stop limit price based on the trade direction.

                    // If the IsAsync flag is true, place the stop limit order asynchronously.
                    if (IsAsync)
                        PlaceStopLimitOrderAsync(OrderTradeType, SymbolName, volumeInUnits, stopLimitPrice, LimitRangeInPips, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod, OnCompleted);
                    // If the IsAsync flag is false, place the stop limit order synchronously.                    
                    else
                        result = PlaceStopLimitOrder(OrderTradeType, SymbolName, volumeInUnits, stopLimitPrice, LimitRangeInPips, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod);

                    break;

                default:
                    Print("Invalid order type");  // Print an error message if the order type is invalid.

                    throw new ArgumentOutOfRangeException("OrderType");  // Throw an exception if the order type is not valid.
            }

            if (!IsAsync) OnCompleted(result);  // If the order is not placed asynchronously, trigger the completion for synchronous execution.
        }

        // This method is called when the trade order is completed, either successfully or with an error.
        private void OnCompleted(TradeResult result)
        {
            if (!result.IsSuccessful) Print("Error: ", result.Error);  // Print an error message if the order was not successful.

            Stop();  // Stop the robot after completing the trade.
        }
    }
}
