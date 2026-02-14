// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This sample cBot listens to bar opening events and performs trade operations based on
//    simple bullish and bearish reversal patterns.
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
    public class CustomHandlersExample : Robot
    {

        // This method is called once at the start of the cBot's execution.
        protected override void OnStart()
        {
            Bars.BarOpened += BullishReversal;  // Add event handler for detecting bullish reversals.
            Bars.BarOpened += BearishReversal;  // Adds event handler for detecting bearish reversals.
        }

        // Handler method to check and execute trades for a bullish reversal.        
        private void BullishReversal(BarOpenedEventArgs args) 
        {
            // Checks for a bullish reversal condition based on previous bar closes.
            if (Bars.LastBar.Open > Bars.Last(1).Close && Bars.LastBar.Open > Bars.Last(2).Close) 
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000, null, 10, 50);  // Places a buy market order if the bullish reversal condition is met.
            }
        }

        // Handler method to check and execute trades for a bearish reversal.
        private void BearishReversal(BarOpenedEventArgs args) 
        {
            // Checks for a bearish reversal condition based on previous bar closes.
            if (Bars.LastBar.Open < Bars.Last(1).Close && Bars.LastBar.Open < Bars.Last(2).Close) 
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000, null, 10, 50);  // Places a sell market order if the bearish reversal condition is met.
            }
        }
                
    }
}

