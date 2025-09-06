// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Parabolic SAR indicator to trigger buy and sell orders. A buy order is placed 
//    when the Parabolic SAR flips below the price and the previous SAR value was above the price. 
//    A sell order is placed when the Parabolic SAR flips above the price and the previous SAR value 
//    was below the price.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.  
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class ParabolicSARSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private ParabolicSAR _parabolicSAR;  // Store the Parabolic SAR indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "ParabolicSARSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Min AF", DefaultValue = 0.02, Group = "Parabolic SAR", MinValue = 0)]
        public double MinAf { get; set; }  // Minimum Acceleration Factor for the Parabolic SAR, default is 0.02.

        [Parameter("Max AF", DefaultValue = 0.2, Group = "Parabolic SAR", MinValue = 0)]
        public double MaxAf { get; set; }  // Maximum Acceleration Factor (AF) for the Parabolic SAR, default is 0.2.


        // This property retrieves all positions opened by this bot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label as this bot.
            }
        }

        // This method is called when the bot starts and is used for initialization.
        protected override void OnStart()
        {
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Parabolic SAR indicator.
            _parabolicSAR = Indicators.ParabolicSAR(MinAf, MaxAf);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the Parabolic SAR is below the current bar and was above the previous bar, it is a buy signal.
            if (_parabolicSAR.Result.Last(0) < Bars.LowPrices.Last(0) && _parabolicSAR.Result.Last(1) > Bars.HighPrices.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // If the Parabolic SAR is above the current bar and was below the previous bar, it is a sell signal.
            else if (_parabolicSAR.Result.Last(0) > Bars.HighPrices.Last(0) && _parabolicSAR.Result.Last(1) < Bars.LowPrices.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
            }
        }

        // This method closes all positions of the specified trade type.
        private void ClosePositions(TradeType tradeType)
        {
            foreach (var position in BotPositions)
            {
                // Check if the position matches the specified trade type before closing.
                if (position.TradeType != tradeType) continue;

                ClosePosition(position);  // Close the position.
            }
        }
    }
}
