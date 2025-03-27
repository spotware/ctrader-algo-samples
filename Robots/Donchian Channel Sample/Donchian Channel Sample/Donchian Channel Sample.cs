// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This example cBot uses the Donchian Channel to identify breakouts. It opens a buy position when the price 
//    crosses above the upper Donchian Channel band and a sell position when the price crosses below the lower 
//    Donchian Channel band. It also manages positions by closing opposite trades based on the breakout signals.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.  
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class DonchianChannelSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private DonchianChannel _donchianChannel;  // Store the Donchian Channel indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Label", DefaultValue = "DonchianChannelSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Periods", DefaultValue = 20, Group = "Donchian Channel", MinValue = 1)]
        public int Periods { get; set; }  // Number of periods for the Donchian Channel, defaulting to 20.

        // This property finds all positions opened by this bot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label used by the bot.
            }
        }

        // This method is called when the bot starts and is used for initialisation.
        protected override void OnStart()
        {
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Donchian Channel indicator with the specified period.
            _donchianChannel = Indicators.DonchianChannel(Periods);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the price crosses below the lower Donchian Channel, execute a buy trade.
            if (Bars.LowPrices.Last(0) <= _donchianChannel.Bottom.Last(0) && Bars.LowPrices.Last(1) > _donchianChannel.Bottom.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open Sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);  // Open a buy market order with the specified volume.
            }

            // If the price crosses above the upper Donchian Channel, execute a Sell trade.
            else if (Bars.HighPrices.Last(0) >= _donchianChannel.Top.Last(0) && Bars.HighPrices.Last(1) < _donchianChannel.Top.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label);  // Open a sell market order with the specified volume.
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
