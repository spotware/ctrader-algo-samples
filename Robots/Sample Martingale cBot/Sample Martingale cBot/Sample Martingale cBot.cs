// ----------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you want to modify this file, please use the "Duplicate" functionality to make a copy. 
//
//    The Sample Martingale cBot creates a random sell or buy order. If the stop loss is hit, a new 
//    order of the same type is created with double the initial volume amount. The cBot will continue
//    to double the volume amount for all orders created until one of them hits the take profit. After 
//    a take profit is hit, a new random buy or sell order is created with the initial volume amount.
//
// ----------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;

namespace cAlgo
{
    // Define the cBot attributes such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleMartingalecBot : Robot
    {
        // Define the input parameters for the cBot.
        [Parameter("Initial Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double InitialQuantity { get; set; }  // Initial trade quantity in lots.

        [Parameter("Stop Loss", Group = "Protection", DefaultValue = 40)]
        public int StopLoss { get; set; }  // Stop loss in pips.

        [Parameter("Take Profit", Group = "Protection", DefaultValue = 40)]
        public int TakeProfit { get; set; }  // Take profit in pips.

        // Private field to generate a random trade type.
        private readonly Random _random = new Random();

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Subscribe to the Positions.Closed event to handle the closing of positions. 
            Positions.Closed += OnPositionsClosed;

            // Execute the first order with the specified initial quantity and a random trade type.
            ExecuteOrder(InitialQuantity, GetRandomTradeType());
        }

        // Method to execute a market order.
        private void ExecuteOrder(double quantity, TradeType tradeType)
        {
            // Convert the trade quantity in lots to the volume in units.
            var volumeInUnits = Symbol.QuantityToVolumeInUnits(quantity);
            
            // Execute a market order with the specified trade type, symbol, volume, label, stop loss and take profit.
            var result = ExecuteMarketOrder(tradeType, SymbolName, volumeInUnits, "Martingale", StopLoss, TakeProfit);

            // If there are no funds to execute the order, stop the cBot.
            if (result.Error == ErrorCode.NoMoney)
                Stop();
        }

        // This event handler is triggered when a position is closed.
        private void OnPositionsClosed(PositionClosedEventArgs args)
        {
            Print("Closed");  // Print a message in the log that the position was closed.

            var position = args.Position;

            // Only handle positions labelled as "Martingale" and for the specified symbol.
            if (position.Label != "Martingale" || position.SymbolName != SymbolName)
                return;

            // If the position was profitable, start a new random trade with the specified initial quantity.
            if (position.GrossProfit > 0)
            {
                ExecuteOrder(InitialQuantity, GetRandomTradeType());
            }
            // If the position was a loss, double the trade quantity and continue with the same trade type.
            else
            {
                ExecuteOrder(position.Quantity * 2, position.TradeType);
            }
        }

        // Method to randomly choose between the buy or sell trade type.
        private TradeType GetRandomTradeType()
        {
            // Return buy or sell randomly.
            return _random.Next(2) == 0 ? TradeType.Buy : TradeType.Sell;
        }
    }
}
