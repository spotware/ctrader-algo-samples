// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Center of Gravity (CoG) indicator to identify potential buy and sell signals 
//    based on crossover points between the CoG Result and Lag lines. It enters buy or sell trades 
//    when crossover conditions are met and manages open positions with specified stop-loss and 
//    take-profit settings.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class CenterOfGravitySample : Robot
    {
        // Private fields for storing indicator and trade volume.        
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private CenterOfGravity _centerOfGravity;  // Store the Center of Gravity indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "CenterOfGravitySample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter(DefaultValue = 10, Group = "Center Of Gravity", MinValue = 1)]
        public int Length { get; set; }  // Length parameter for calculating the Center of Gravity, default is 10.

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

            // Initialise the Center of Gravity indicator with the specified length.
            _centerOfGravity = Indicators.CenterOfGravity(Length);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Check if the Center of Gravity Result line has crossed above the Lag line.
            if (_centerOfGravity.Result.Last(0) > _centerOfGravity.Lag.Last(0) && _centerOfGravity.Result.Last(1) <= _centerOfGravity.Lag.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // Check if the Center of Gravity Result line has crossed below the Lag line
            else if (_centerOfGravity.Result.Last(0) < _centerOfGravity.Lag.Last(0) && _centerOfGravity.Result.Last(1) >= _centerOfGravity.Lag.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any buy positions.

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
