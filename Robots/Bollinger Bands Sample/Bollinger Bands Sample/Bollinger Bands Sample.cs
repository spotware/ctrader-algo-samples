// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses Bollinger Bands to identify potential overbought or oversold conditions. 
//    When the price closes below the lower band, it opens a buy order, and when it closes above the upper band, it opens a sell order.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class BollingerBandsSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store trade volume in units calculated based on the specified lot size.

        private BollingerBands _bollingerBands;  // Store the Bollinger Bands indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01.

        [Parameter("Label", DefaultValue = "BollingerBandsSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Source", Group = "Bollinger Bands")]
        public DataSeries Source { get; set; }  // Data series used as the source for Bollinger Bands calculation.

        [Parameter(DefaultValue = 20, Group = "Bollinger Bands", MinValue = 1)]
        public int Periods { get; set; }  // Period for calculating the Bollinger Bands, default is 20.

        [Parameter("Standard Dev", DefaultValue = 2.0, Group = "Bollinger Bands", MinValue = 0.0001, MaxValue = 10)]
        public double StandardDeviations { get; set; }  // Standard deviation multiplier for the bands, default is 2.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Bollinger Bands")]
        public MovingAverageType MAType { get; set; }  // Type of moving average used in Bollinger Bands, default is Simple.

        [Parameter("Shift", DefaultValue = 0, Group = "Bollinger Bands", MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }  // Shift of Bollinger Bands, default is 0.

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

            // Initialise the Bollinger Bands indicator with specified parameters.
            _bollingerBands = Indicators.BollingerBands(Source, Periods, StandardDeviations, MAType);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Check if the price has crossed below the lower Bollinger Band.
            if (Bars.LowPrices.Last(0) <= _bollingerBands.Bottom.Last(0) && Bars.LowPrices.Last(1) > _bollingerBands.Bottom.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);  // Open a new buy market order.
            }
            else if (Bars.HighPrices.Last(0) >= _bollingerBands.Top.Last(0) && Bars.HighPrices.Last(1) < _bollingerBands.Top.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label);  // Open a new sell market order.
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
