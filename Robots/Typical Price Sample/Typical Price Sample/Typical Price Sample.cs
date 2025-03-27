// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates the use of the Typical Price indicator and Simple Moving Average (SMA)
//    for trading decisions. It opens buy or sell orders when crossovers occur between the Typical 
//    Price and its SMA.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class TypicalPriceSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private TypicalPrice _typicalPrice;  // Store the Typical Price indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "TypicalPriceSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        // This property finds all positions opened by this cBot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label used by the cBot.
            }
        }

        // This method is called when the cBot starts and is used for initialisation.
        protected override void OnStart()
        {
            // Convert the specified volume in lots to volume in units for the trading symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Typical Price indicator.
            _typicalPrice = Indicators.TypicalPrice();

            // Initialise the SMA using the Typical Price result with a 14-period setting.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(_typicalPrice.Result, 14);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the Typical Price crosses above the SMA.
            if (_typicalPrice.Result.HasCrossedAbove(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any existing sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Check if the Typical Price crosses below the SMA.
            else if (_typicalPrice.Result.HasCrossedBelow(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Buy);  // Close any existing buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to sell with the specified volume, stop loss and take profit.
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
