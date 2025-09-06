// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates partial closing of positions at predefined levels. When a position reaches 
//    a specified pip value, a portion of the position is closed based on a percentage defined by the 
//    user. This allows for securing partial profits while letting the remaining position run.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;
using System.Collections.Generic;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PartialCloseSample : Robot
    {
        // Private fields for tracking closed positions at different levels.
        private readonly List<long> _firstLevelClosedPositions = new List<long>();
        private readonly List<long> _secondLevelClosedPositions = new List<long>();

        // Parameters for first-level partial close.
        [Parameter("Close %", DefaultValue = 20, Group = "First Level")]
        public double FirstLevelCloseAmountInPercentage { get; set; }  // Percentage to close at first level, default is 20.

        [Parameter("Pips", DefaultValue = 20, Group = "First Level")]
        public double FirstLevelClosePips { get; set; }  // Pips required to trigger the first-level close, default is 20.

        [Parameter("Close %", DefaultValue = 20, Group = "Second Level")]
        public double SecondLevelCloseAmountInPercentage { get; set; }  // Percentage to close at second level, default is 20.

        [Parameter("Pips", DefaultValue = 35, Group = "Second Level")]
        public double SecondLevelClosePips { get; set; }  // Pips required to trigger the second-level close, default is 35.

        // This method is called when the bot starts and is used for initialisation.
        protected override void OnStart()
        {
            FirstLevelCloseAmountInPercentage /= 100;  // Convert first level percentage to a decimal.
            SecondLevelCloseAmountInPercentage /= 100;  // Convert second level percentage to a decimal.

            Positions.Opened += Positions_Opened;  // Attach event handler for position opened.
            Positions.Closed += Positions_Closed;  // Attach event handler for position closed.
        }

        // This method handles actions when a position is closed.
        private void Positions_Closed(PositionClosedEventArgs obj)
        {

            // Check if the position is fully closed and no longer exists in the open positions list.
            if (Positions.Any(position => position.Id == obj.Position.Id) == false)
            {
                // If the position was tracked in the first-level list, clean its enteries from ID collections.
                if (_firstLevelClosedPositions.Contains(obj.Position.Id))
                {
                    _firstLevelClosedPositions.Remove(obj.Position.Id);
                }

                // If the position was tracked in the second-level list, clean its enteries from ID collections.
                if (_secondLevelClosedPositions.Contains(obj.Position.Id))
                {
                    _secondLevelClosedPositions.Remove(obj.Position.Id);
                }
            }

            // If there are other positions from same symbol then do not remove the symbol Tick event handler.
            if (Positions.Any(position => position.SymbolName.Equals(obj.Position.SymbolName, StringComparison.Ordinal)))
            {
                return;
            }

            // If there is no other position from the closed position symbol then remove the Tick event handler.
            var positionSymbol = Symbols.GetSymbol(obj.Position.SymbolName);

            positionSymbol.Tick -= PositionSymbol_Tick;
        }

        // This method is called when a position is opened.
        private void Positions_Opened(PositionOpenedEventArgs obj)
        {
            // If there are other positions from same symbol then do not add the symbol Tick event handler.
            // Because we already have one.
            if (Positions.Count(position => position.SymbolName.Equals(obj.Position.SymbolName, StringComparison.Ordinal)) > 1)
            {
                return;
            }

            // Add position symbol tick event handler.
            var positionSymbol = Symbols.GetSymbol(obj.Position.SymbolName);

            positionSymbol.Tick += PositionSymbol_Tick;
        }

        // This method is triggered when a tick event occurs for a symbol.
        private void PositionSymbol_Tick(SymbolTickEventArgs obj)
        {
            // Get all positions for the symbol that triggered the tick event.
            var symbolPositions = Positions.Where(position => position.SymbolName.Equals(obj.SymbolName, StringComparison.Ordinal)).ToArray();

            // Loop through each position for the symbol.
            foreach (var position in symbolPositions)
            {
                // If the first level closing condition is met (position is not already closed and Pips exceed the threshold).
                if (_firstLevelClosedPositions.Contains(position.Id) == false && position.Pips >= FirstLevelClosePips)
                {
                    // Close a portion of the position based on the percentage defined for the first level.
                    ClosePositionByVolumePercenatage(position, FirstLevelCloseAmountInPercentage);

                    // Add the position ID to the list of closed positions for the first level to avoid re-closing.
                    _firstLevelClosedPositions.Add(position.Id);
                }

                // If the second level closing condition is met (position is not already closed and Pips exceed the second level threshold).
                else if (_secondLevelClosedPositions.Contains(position.Id) == false && position.Pips >= SecondLevelClosePips)
                {
                    // Close a portion of the position based on the percentage defined for the second level.
                    ClosePositionByVolumePercenatage(position, SecondLevelCloseAmountInPercentage);

                    // Add the position ID to the list of closed positions for the second level to avoid re-closing.
                    _secondLevelClosedPositions.Add(position.Id);
                }
            }
        }

        // This method closes a specified percentage of the position volume.
        private void ClosePositionByVolumePercenatage(Position position, double volumePercent)
        {
            // Get the symbol for the position.
            var symbol = Symbols.GetSymbol(position.SymbolName);

            // Calculate the volume to close based on the percentage of the position total volume.
            var volumeToClose = symbol.NormalizeVolumeInUnits(position.VolumeInUnits * volumePercent);

            // Close the calculated portion of the position.
            ClosePosition(position, volumeToClose);
        }
    }
}
