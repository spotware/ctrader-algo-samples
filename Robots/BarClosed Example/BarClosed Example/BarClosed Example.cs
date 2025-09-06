// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot checks if the closing price of the last bar is more than 0.5% above its low. If true, 
//    it closes all open positions and opens a buy position with a 50 pip stop loss.
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
    // Define the cBot class with no specific access rights.
    [Robot(AccessRights = AccessRights.None)]
    public class BarClosedExample : Robot
    {
        // This method is triggered when a bar is closed.
        protected override void OnBarClosed() 
        {
            // Calculate the percentage difference between the closing price and the low price of the last bar.
            var lowCloseDifference = ((Bars.LastBar.Close - Bars.LastBar.Low) / Bars.LastBar.Close) * 100;

            // Check if the percentage difference is greater than 0.5% (i.e., the bar closed significantly above its low).
            if (lowCloseDifference > 0.5) 
            {
                // If the condition is met, close all open positions.
                foreach (var position in Positions) 
                {
                    position.Close();  // Close the position.
                }
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000, null, null, 50);  // Place a new buy market order with a volume of 10,000 units and a stop loss of 50 pips.

            }
        }
    }
}
