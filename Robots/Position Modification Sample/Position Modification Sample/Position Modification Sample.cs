// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to modify an existing position by adjusting stop loss, 
//    take profit, trailing stop or volume. It identifies the position using label or comment 
//    parameters and applies user-defined modifications.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionModificationSample : Robot
    {
        [Parameter("Position Comment")]
        public string PositionComment { get; set; }  // Comment to identify the position.

        [Parameter("Position Label")]
        public string PositionLabel { get; set; }  // Label to identify the position.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Stop Loss Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; }  // Stop loss trigger method, default is Trade.

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Volume in lots for the trade, defaulting to 0.01 lots.

        [Parameter("Has Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }  // Whether to use a trailing stop, default value is false.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            Position position = null;

            // Check if both position comment and label are provided for identification.
            if (!string.IsNullOrWhiteSpace(PositionComment) && !string.IsNullOrWhiteSpace(PositionLabel))
            {
                // Find a position that matches both the label and comment provided.
                position = Positions.FindAll(PositionLabel).FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            
            // Check if only the position comment is provided for identification.
            else if (!string.IsNullOrWhiteSpace(PositionComment))
            {
                // Find the first position with a matching comment.
                position = Positions.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            
            // Check if only the position label is provided for identification.
            else if (!string.IsNullOrWhiteSpace(PositionLabel))
            {
                // Find a position with the matching label.
                position = Positions.Find(PositionLabel);
            }

            // If no position is found, output an error message and stops the cBot.
            if (position == null)
            {
                Print("Couldn't find the position, please check the comment and label");
                Stop();
            }

            var positionSymbol = Symbols.GetSymbol(position.SymbolName);  // Retrieve the symbol information of the identified position.

            var stopLossInPrice = position.StopLoss;  // Initialise the stop loss price variable with the current position stop loss.

            // Check if the user has defined a stop loss in pips.
            if (StopLossInPips > 0)
            {
                // Convert the stop loss in pips to price.
                var stopLossInPipsPrice = StopLossInPips * positionSymbol.PipSize;

                // Calculate the stop loss price based on the trade type.
                stopLossInPrice = position.TradeType == TradeType.Buy ? position.EntryPrice - stopLossInPipsPrice : position.EntryPrice + stopLossInPipsPrice;
            }

            var takeProfitInPrice = position.TakeProfit;  // Initialise the take profit price variable with the current position's take profit.

            // Check if the user has defined a take profit in pips.
            if (TakeProfitInPips > 0)
            {
                // Convert the take profit in pips to price.
                var takeProfitInPipsPrice = TakeProfitInPips * positionSymbol.PipSize;

                // Calculate the take profit price based on the trade type.
                takeProfitInPrice = position.TradeType == TradeType.Buy ? position.EntryPrice + takeProfitInPipsPrice : position.EntryPrice - takeProfitInPipsPrice;
            }

            // Modify the position with the new stop loss, take profit, trailing stop and stop loss trigger method.
            ModifyPosition(position, stopLossInPrice, takeProfitInPrice, HasTrailingStop, StopLossTriggerMethod);

            // Check if the user has specified a new volume in lots.
            if (VolumeInLots > 0)
            {
                // Convert the volume in lots to units.
                var volumeInUnits = positionSymbol.QuantityToVolumeInUnits(VolumeInLots);

                // Update the position with the new volume.
                ModifyPosition(position, volumeInUnits);
            }
        }
    }
}
