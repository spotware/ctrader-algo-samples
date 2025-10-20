// -------------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    All changes to this file might be lost on the next application update.
//    If you want to modify this file, please use the "Duplicate" functionality to make a copy. 
//
//    The Sample Grid cBot uses a grid strategy to open positions in a defined trade direction (buy
//    or sell) at intervals determined by the steps parameter. It continues to open new positions as  
//    long as the distance between the last position and the current price exceeds the step size and   
//    there are sufficient funds. Once the total profit from all open positions meets or exceeds the 
//    target profit, the cBot closes all positions and stops running. 
//
// -------------------------------------------------------------------------------------------------------

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
    // Define the cBot attributes. No additional access right is provided in this case.
    [Robot(AccessRights = AccessRights.None)]
    public class SampleGridcBot : Robot
    {
        // Define the input parameters for the cBot.
        [Parameter("Volume (lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots.

        [Parameter("Trade Side")]
        public TradeType TradeType { get; set; }  // Trade direction (buy or sell).

        [Parameter("Step (pips)", DefaultValue = 5, MinValue = 0.1, Step = 0.1)]
        public double StepPips { get; set; }  // Step size in pips to open the next grid position.

        [Parameter("Target Profit", DefaultValue = 20)]
        public double TargetProfit { get; set; }  // Target profit for the grid in the account currency.

        // Flag to indicate whether there is enough money to open new positions.
        private bool _enoughMoney = true;

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // If there are no grid positions at the start, open the first position.
            if (GridPositions.Length == 0)
                OpenPosition();
        }

        // This method is called on every price tick.
        protected override void OnTick()
        {
            // Check if the total net profit of all grid positions exceeds the target profit.
            if (GridPositions.Sum(p => p.NetProfit) >= TargetProfit)
            {
                Print("Target profit is reached. Closing all grid positions");  // Print a message indicating that the target profit has been reached and all grid positions are closing.
                CloseGridPositions();  // Close all existing positions once the target profit is reached.
                Print("All grid positions are closed. Stopping cBot");  // Print a message confirming that all grid positions have been closed and the cBot is stopping.
                Stop();  // Stop the cBot after closing all positions.
            }

            // Check whether there are open grid positions and the funds are sufficient to continue trading.
            if (GridPositions.Length > 0 && _enoughMoney)
            {
                // Find the last position in the grid based on the highest number of pips.
                var lastGridPosition = GridPositions.OrderBy(p => p.Pips).Last();

                // Calculate the distance in pips from the last grid position.
                var distance = CalculateDistanceInPips(lastGridPosition);

                // If the price has moved by more than the defined step size, open a new grid position.
                if (distance >= StepPips)
                    OpenPosition();
            }
        }

        // Property to get all existing grid positions (buy or sell) for the current symbol.
        private Position[] GridPositions
        {
            get
            {
                // Filter all positions that match the current symbol and trade type.
                return Positions
                    .Where(p => p.SymbolName == SymbolName && p.TradeType == TradeType)
                    .ToArray();
            }
        }

        // Method to calculate the distance in pips between the last position and the current price.
        private double CalculateDistanceInPips(Position position)
        {
            // For buy positions, calculate the distance from the entry price to the current ask price.
            if (position.TradeType == TradeType.Buy)
                return (position.EntryPrice - Symbol.Ask) / Symbol.PipSize;
            // For sell positions, calculate the distance from the entry price to the current bid price.
            else
                return (Symbol.Bid - position.EntryPrice) / Symbol.PipSize;
        }

        // Method to open a new grid position.
        private void OpenPosition()
        {
            // Execute a market order with the specified trade type, symbol, volume and label.
            var result = ExecuteMarketOrder(TradeType, SymbolName, Symbol.QuantityToVolumeInUnits(VolumeInLots), "Grid");

            // If there is no funds to open the position, set the flag and print a message.
            if (result.Error == ErrorCode.NoMoney)
            {
                _enoughMoney = false;
                Print("Not enough money to open additional positions");
            }
        }

        // Method to close all grid positions.
        private void CloseGridPositions()
        {
            // Continue closing grid positions until all of them are closed.
            while (GridPositions.Length > 0)
            {
                foreach (var position in GridPositions)
                {
                    ClosePosition(position);  // Close each position in the grid.
                }
            }
        }
    }
}