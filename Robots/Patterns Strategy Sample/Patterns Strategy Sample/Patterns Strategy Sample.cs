// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example cBot trades the hammer pattern for long entries and the hanging man pattern for short entries.
//
//    For a detailed tutorial on creating this cBot, watch the video at: https://youtu.be/mEoIvP11Z1U
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as AccessRights and its ability to add indicators.  
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class PatternsStrategySample : Robot
    {
        [Parameter(DefaultValue = 1000)]
        public double Volume { get; set; }  // Parameter for the trading volume, default value is 1000.

        [Parameter(DefaultValue = 10)]
        public double StopLoss { get; set; }  // Parameter for the stop loss in pips, default value is 10.

        [Parameter(DefaultValue = 10)]
        public double TakeProfit { get; set; }  // Parameter for the take profit in pips, default value is 10.

        protected override void OnStart()
        {

        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Check for Hammer pattern (bullish) on the last bar.
            if (Bars.Last(0).Close == Bars.Last(0).High &&  // Close price is equal to High.
               (Bars.Last(0).Close - Bars.Last(0).Open) < (Bars.Last(0).Close - Bars.Last(0).Low) * 0.2)  // The body is less than 20% of the total bar size, with a longer lower wick.
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);  // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // Check for Hanging Man pattern (bearish) on the last bar.
            if (Bars.Last(0).Close == Bars.Last(0).Low &&  // Close price is equal to Low.
               (Bars.Last(0).Open - Bars.Last(0).Close) < (Bars.Last(0).High - Bars.Last(0).Close) * 0.2)  // The body is less than 20% of the total bar size, with a longer upper wick.
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);  // Open a sell market order with the specified volume, stop loss and take profit.
            }

        }

        protected override void OnStop()
        {

        }
    }
}
