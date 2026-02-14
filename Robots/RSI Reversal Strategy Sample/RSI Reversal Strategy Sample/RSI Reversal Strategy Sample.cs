// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example cBot implements a strategy based on the Relative Strength Index (RSI) indicator reversal. 
//
//    For a detailed tutorial on creating this cBot, watch the video at: https://youtu.be/mEoIvP11Z1U
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as AccessRights and its ability to add indicators.
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class RSIReversalStrategySample : Robot
    {
        // Define input parameters for the cBot.
        [Parameter(DefaultValue = 30)]
        public int BuyLevel { get; set; }  // Buy level, below which the cBot opens a buy position, defaulting to 30.

        [Parameter(DefaultValue = 70)]
        public int SellLevel { get; set; }  // Sell level, above which the cBot opens a sell position, defaulting to 70.

        private RelativeStrengthIndex _rsi;  // Store the Relative Strength Index indicator.

        // This method is called when the cBot starts and is used for initialisation.
        protected override void OnStart()
        {
            // Initialize the RSI indicator with a period of 14 using the closing prices of the bars.
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // If the RSI value is below the Buy level, open a buy trade.
            if (_rsi.Result.LastValue < BuyLevel)
            {
                // Ensure there are no open positions before opening a buy trade.
                if (Positions.Count == 0)
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, 1000);  // Open a buy market order with 1000 units.
                
                // Close any existing sell positions.
                foreach (var position in Positions.Where(p => p.TradeType == TradeType.Sell))
                {
                    position.Close();
                }

            }
            
            // If the RSI value is above the Sell level, open a sell trade.
            else if (_rsi.Result.LastValue > SellLevel)
            {
                // Ensure there are no open positions before opening a sell trade.
                if (Positions.Count == 0)
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, 1000);  // Open a sell market order with 1000 units.
                
                // Close any existing buy positions.
                foreach (var position in Positions.Where(p => p.TradeType == TradeType.Buy))
                {
                    position.Close();
                }
            }
        }

        protected override void OnStop()
        {

        }
    }
}
