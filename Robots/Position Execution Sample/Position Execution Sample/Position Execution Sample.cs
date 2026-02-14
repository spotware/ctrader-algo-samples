// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to execute a market order based on user-defined parameters,
//    supporting both synchronous and asynchronous operations. It also handles trailing stops and
//    other advanced trading features.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionExecutionSample : Robot
    {
        [Parameter("Direction", DefaultValue = TradeType.Buy)]
        public TradeType Direction { get; set; }  // The trade direction, default is Buy.

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Volume to trade in lots, default is 0.01.

        [Parameter("Distance (Pips)", DefaultValue = 20, MinValue = 1)]
        public double DistanceInPips { get; set; }  // Distance in pips for price levels, defaulting to 20 pips.

        [Parameter("Stop (Pips)", DefaultValue = 10, MinValue = 0)]
        public double StopInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Target (Pips)", DefaultValue = 10, MinValue = 0)]
        public double TargetInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Comment")]
        public string Comment { get; set; }  // Additional text to attach to the order.

        [Parameter("Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }  // Enable trailing stop, default is false.

        [Parameter("Stop Loss Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; }  // Triggering mechanism for the stop loss, default is Trade.

        [Parameter("Async", DefaultValue = false)]
        public bool IsAsync { get; set; }  // Place the order asynchronously if true, default is false.

        // The main method executed when the cBot starts.
        protected override void OnStart()
        {
            // Convert the specified volume in lots to volume in units for the trading symbol.
            var volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            DistanceInPips *= Symbol.PipSize;  // Adjusts the distance in pips to price.

            // Set stop-loss and take-profit values or keep the original.
            var stopLoss = StopInPips == 0 ? null : (double?)StopInPips;
            var takeProfit = TargetInPips == 0 ? null : (double?)TargetInPips;

            TradeResult result = null;

            // If the IsAsync flag is true, place the limit order asynchronously.
            if (IsAsync)
                ExecuteMarketOrderAsync(Direction, SymbolName, volumeInUnits, Label, stopLoss, takeProfit, Comment, HasTrailingStop, StopLossTriggerMethod, OnCompleted);
            // If the IsAsync flag is false, place the limit order synchronously.
            else
                result = ExecuteMarketOrder(Direction, SymbolName, volumeInUnits, Label, stopLoss, takeProfit, Comment, HasTrailingStop, StopLossTriggerMethod);

            if (!IsAsync) OnCompleted(result);  // If the order is not placed asynchronously, trigger the completion for synchronous execution.
        }

        private void OnCompleted(TradeResult result)
        {
            if (!result.IsSuccessful) Print("Error: ", result.Error);  // Logs the error if the order fails.

            Stop();  // Stops the cBot execution.
        }
    }
}
