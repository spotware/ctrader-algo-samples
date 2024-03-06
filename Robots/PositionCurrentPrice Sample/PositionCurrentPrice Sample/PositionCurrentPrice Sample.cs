// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    On start, this sample executes a market order for the current symbol. Whenever a bar closes,
//    the cBot checks if the position current price is greater to its open price. If it is indeed 
//    greater, the cBot hedges the position by placing a limit order above the current price.
//    If it is lower, the cBot closes the unprofitable position and places a limit order
//    below the current price.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class PositionCurrentPriceSample : Robot
    {


        protected override void OnStart()
        {
            // Executing a market order so that
            // there is at least one initial position
            ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000, "cbot position");
        }

        protected override void OnBarClosed()
        {
            // Finding all positions opened by the cBot
            var cbotPositions = Positions.FindAll("cbot position");
            
            // Iterating over all positions opened by the cBot
            foreach (var position in cbotPositions)
            {
                if (position.CurrentPrice > position.EntryPrice)
                {
                    // Placing a limit order in the opposite direction
                    // and above the current price if the current price
                    // is greater than the entry price
                    PlaceLimitOrder(TradeType.Sell, SymbolName, 20000, position.CurrentPrice * 1.05, "cbot position");
                }
                else
                {
                    position.Close();
                    
                    // Placing a limit order in the opposite direction
                    // and below the current price if the current price
                    // is smaller than the entry price
                    PlaceLimitOrder(TradeType.Buy, SymbolName, 20000, position.CurrentPrice * 0.95, "cbot position");
                }
            }
        }


    }
}