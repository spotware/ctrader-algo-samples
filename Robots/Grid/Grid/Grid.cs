// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This sample cBot implements a simple grid trading strategy. It opens a series of positions
//    based on a fixed pip step size and closes all positions when a predefined profit target is reached.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as access rights.
    [Robot(AccessRights = AccessRights.None)]
    public class Grid : Robot
    {
        // Define input parameters for the cBot.
        [Parameter("Volume (lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Trade Side")]
        public TradeType TradeType { get; set; }  // Direction of trades (Buy or Sell).

        [Parameter("Step (pips)", DefaultValue = 5, MinValue = 0.1, Step = 0.1)]
        public double StepPips { get; set; } // Distance in pips between grid positions, default is 5.

        [Parameter("Target Profit", DefaultValue = 20)]
        public double TargetProfit { get; set; } // Total net profit target for closing the grid, default is 20.

        private bool enoughMoney = true;  // Flag to check if sufficient funds are available.

        // This method is called when the bot starts.
        protected override void OnStart()
        {
            // Open the first grid position if none exist.
            if (GridPositions.Length == 0)
                OpenPosition();
        }

        // This method is called on every tick to manage the grid.
        protected override void OnTick()
        {
            // Check if the total net profit meets or exceeds the target profit.
            if (GridPositions.Sum(p => p.NetProfit) >= TargetProfit)
            {
                Print("Target profit is reached. Closing all grid positions");  // Log that the profit target has been reached and positions will be closed.
                CloseGridPositions();  // Close all active positions in the grid.
                Print("All grid positions are closed. Stopping cBot");  // Log that all positions are closed and the cBot will stop.
                Stop();  // Stop the execution of the cBot.
            }

            // Open a new position if the distance from the last position exceeds the step size.
            if (GridPositions.Length > 0 && enoughMoney)
            {
                var lastGridPosition = GridPositions.OrderBy(p => p.Pips).Last();  // Find the position farthest from the current price in terms of pips.
                var distance = CalculateDistanceInPips(lastGridPosition);  // Calculate the distance from the last position to the current price in pips.

                // Open a new position if the distance exceeds the defined step size.
                if (distance >= StepPips)
                    OpenPosition();
            }
        }

        // Return all grid positions for the current symbol and trade type.
        private Position[] GridPositions
        {
            get
            {
                // Filter and return positions matching the symbol name and trade type (buy or sell).
                return Positions
                    .Where(p => p.SymbolName == SymbolName && p.TradeType == TradeType)
                    .ToArray();
            }
        }

        // Calculate the distance in pips from a given position to the current price.
        private double CalculateDistanceInPips(Position position)
        {
            // For a buy position, calculate the distance between entry price and current Ask price.
            if (position.TradeType == TradeType.Buy)
                return (position.EntryPrice - Symbol.Ask) / Symbol.PipSize;
            // For a sell position, calculate the distance between entry price and current Bid price.
            else
                return (Symbol.Bid - position.EntryPrice) / Symbol.PipSize;
        }

        // Open a new position in the grid.
        private void OpenPosition()
        {
            // Execute a market order for the specified trade type and symbol with the defined volume.
            var result = ExecuteMarketOrder(TradeType, SymbolName, Symbol.QuantityToVolumeInUnits(VolumeInLots), "Grid");

            // Check if there is an error due to insufficient funds.
            if (result.Error == ErrorCode.NoMoney)
            {
                // Set the flag to indicate that no more money is available to open positions.
                enoughMoney = false;

                // Log that there is not enough money to continue opening positions.
                Print("Not enough money to open additional positions");
            }
        }

        // Closes all positions in the grid.
        private void CloseGridPositions()
        {
            // Loop until all grid positions are closed.
            while (GridPositions.Length > 0)
            {
                foreach (var position in GridPositions)
                {
                    // Close each position in the grid.
                    ClosePosition(position);
                }
            }
        }
    }
}
