// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot implements a simple range-based trading strategy that opens a buy trade when the price 
//    closes higher than it opened and the previous bar closed lower. It opens a sell trade when the 
//    current bar closes lower than it opened and the previous bar closed higher.
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
    // Define the cBot attributes, such as AccessRights.
    [Robot(AccessRights = AccessRights.None)]
    public class RangeStrategyExample : Robot
    {
        [Parameter(DefaultValue = 100000)]
        public double Volume { get; set; }  // Trade volume in units, default is 100000 units.

        [Parameter(DefaultValue = 20)]
        public double StopLoss { get; set; }  // Stop loss in pips, default is 20 pips.

        [Parameter(DefaultValue = 20)]
        public double TakeProfit { get; set; }  // Take profit in pips, default is 20 pips.

        protected override void OnStart()
        {

        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // If the current bar closes higher than it opened and the previous bar closed lower, execute a buy order.
            if (Bars.Last(0).Close > Bars.Last(0).Open && Bars.Last(1).Close < Bars.Last(1).Open &&
            Bars.Last(0).Close > Bars.Last(1).Open)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }

            // If the current bar closes lower than it opened and the previous bar closed higher, execute a sell order.
            if (Bars.Last(0).Close < Bars.Last(0).Open && Bars.Last(1).Close > Bars.Last(1).Open &&
            Bars.Last(0).Close < Bars.Last(1).Open)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);  // Open a market order to sell with the specified volume, stop loss and take profit.
            }
        }

        protected override void OnStop()
        {
            // Handle cBot stop here.
        }
    }
}
