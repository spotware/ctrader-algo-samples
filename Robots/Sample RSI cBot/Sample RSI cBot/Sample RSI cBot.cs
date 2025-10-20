// ---------------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you want to modify this file, please use the "Duplicate" functionality to make a copy. 
//
//    The Sample RSI cBot creates a buy order when the Relative Strength Index indicator crosses level 30 
//    and a sell order when the RSI indicator crosses level 70. The order is closed by a stop loss defined  
//    in the stop loss parameter or by the opposite RSI crossing signal (buy orders close when the RSI crosses   
//    the 70 level, and sell orders are closed when the RSI crosses the 30 level). The cBot can generate only 
//    one buy or sell order at any given time.
//
// ---------------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class SampleRSIcBot : Robot
    {
        // Define input parameters for the cBot.
        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }  // Trade quantity in lots.

        [Parameter("Source", Group = "RSI")]
        public DataSeries Source { get; set; }  // Data series for the RSI indicator.

        [Parameter("Periods", Group = "RSI", DefaultValue = 14)]
        public int Periods { get; set; }  // Number of periods to calculate RSI.

        // Private field for the RSI indicator.
        private RelativeStrengthIndex _rsi;

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Initialise the RSI indicator with the specified source and periods.
            _rsi = Indicators.RelativeStrengthIndex(Source, Periods);
        }

        // This method is called on every tick. The RSI is recalculated on every tick.
        protected override void OnTick()
        {
            // Check if the RSI value is below 30 (oversold condition), signalling a buy opportunity.
            if (_rsi.Result.LastValue < 30)
            {
                Close(TradeType.Sell);  // Close any open sell positions.
                Open(TradeType.Buy);  // Open a new buy position.
            }
            // Check if the RSI value is above 70 (overbought condition), signalling a sell opportunity.
            else if (_rsi.Result.LastValue > 70)
            {
                Close(TradeType.Buy);  // Close any open buy position.
                Open(TradeType.Sell);  // Open a new sell position.
            }
        }

        // Method to close all positions of the specified trade type (buy or sell). It ensures that opposite positions (e.g., sell positions when a buy is triggered) are closed before new ones are opened.
        private void Close(TradeType tradeType)
        {
            // Iterate over all positions that match the "SampleRSI" label, specified symbol and trade type.
            foreach (var position in Positions.FindAll("SampleRSI", SymbolName, tradeType))
                ClosePosition(position);  // Close each found position.
        }

        // Method to open a new position of the specified trade type (buy or sell). It checks for open positions before placing new orders so that duplicate trades are avoided.
        private void Open(TradeType tradeType)
        {
            // Check if there is an existing position with the same label, symbol and trade type.
            var position = Positions.Find("SampleRSI", SymbolName, tradeType);
            
            // Convert trade quantity in lots to volume in units.
            var volumeInUnits = Symbol.QuantityToVolumeInUnits(Quantity);  

            // If no existing position is found, execute a new market order.
            if (position == null)
                ExecuteMarketOrder(tradeType, SymbolName, volumeInUnits, "SampleRSI");
        }
    }
}
