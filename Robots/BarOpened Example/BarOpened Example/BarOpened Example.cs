// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot evaluates the price difference between the opening prices of the current and previous bars. 
//    If the price difference is greater than 1%, it opens a buy order. If the difference is less than -1%, 
//    it opens a sell order. Otherwise, it closes all open positions.
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
    public class BarOpenedExample : Robot
    {
        // This method is triggered when a new bar is created.
        protected override void OnBar() 
        {
            // Get the previous bar's data (the bar before the most recent one).
            var previousBar = Bars[Bars.Count - 2];

            // Calculate the percentage difference between the current and previous bar's opening price.
            var priceDifference = ((Bars.LastBar.Open - previousBar.Open) / previousBar.Open) * 100;
            
            // If price difference is greater than 1%, place a buy order.
            if (priceDifference > 1) 
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000);  // Place a new buy market order with a volume of 10,000 units.
            }

            // If price difference is less than -1%, execute a sell order.
            else if (priceDifference < -1) 
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000);  // Place a new sell market order with a volume of 10,000 units.
            }

            // If the price difference is between -1% and 1%, close all open positions.
            else 
            {
                foreach (var position in Positions) 
                {
                    position.Close();  // Close the position.
                }
            }
        }
    }
}
