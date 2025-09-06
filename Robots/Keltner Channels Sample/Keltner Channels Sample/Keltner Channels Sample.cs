// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Keltner Channels indicator to trade breakouts. It opens a buy position when
//    the price crosses above the lower band of the channel, and a sell position when the price 
//    crosses below the upper band.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class KeltnerChannelsSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private KeltnerChannels _keltnerChannels;  // Store the Keltner Channels indicator instance.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, with a default of 0.01 lots.

        [Parameter("Label", DefaultValue = "KeltnerChannelsSample")]
        public string Label { get; set; }  // Unique label for identifying orders opened by this bot.

        [Parameter("MA Period", DefaultValue = 20, Group = "Keltner Channels", MinValue = 1)]
        public int MAPeriod { get; set; }  // Moving Average period with default value 20 periods.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Keltner Channels")]
        public MovingAverageType MAType { get; set; }  // Moving Average type with default value Simple.

        [Parameter("ATR Period", DefaultValue = 10, Group = "Keltner Channels", MinValue = 1)]
        public int AtrPeriod { get; set; }  // ATR calculation period with default value 10 periods.

        [Parameter("ATR MA Type", DefaultValue = MovingAverageType.Simple, Group = "Keltner Channels")]
        public MovingAverageType AtrMAType { get; set; }  // ATR Moving Average type with default value Simple.

        [Parameter("Band Distance", DefaultValue = 2.0, MinValue = 0)]
        public double BandDistance { get; set; }  // Multiplier for the ATR to calculate channel width with default value 2.0.

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
            // Convert the specified lot size to volume in units for the trading symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Keltner Channels indicator with the specified parameters.
            _keltnerChannels = Indicators.KeltnerChannels(MAPeriod, MAType, AtrPeriod, AtrMAType, BandDistance);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Check if the price crosses above the lower band indicating a buy signal.
            if (Bars.LowPrices.Last(0) <= _keltnerChannels.Bottom.Last(0) && Bars.LowPrices.Last(1) > _keltnerChannels.Bottom.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);  // Open a buy market order.
            }

            // Check if the price crosses below the upper band indicating a sell signal.
            else if (Bars.HighPrices.Last(0) >= _keltnerChannels.Top.Last(0) && Bars.HighPrices.Last(1) < _keltnerChannels.Top.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label);  // Open a sell market order.
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
