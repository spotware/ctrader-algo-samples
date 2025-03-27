// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot utilizes the Detrended Price Oscillator (DPO) to identify buy and sell signals
//    based on oscillator crossovers with the zero line. When the DPO crosses above zero, the bot 
//    closes any existing sell positions and opens a buy order. Conversely, when the DPO crosses 
//    below zero, it closes any existing buy positions and opens a sell order.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class DetrendedPriceOscillatorSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private DetrendedPriceOscillator _detrendedPriceOscillator;  // Store the DPO indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, default of 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, default of 10 pips.

        [Parameter("Label", DefaultValue = "DetrendedPriceOscillatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Periods", DefaultValue = 21, Group = "Detrended Price Oscillator", MinValue = 1)]
        public int Periods { get; set; }  // Number of periods, default of 21 periods.

        [Parameter("MA Type", Group = "Detrended Price Oscillator")]
        public MovingAverageType MAType { get; set; }  // Type of moving average for the DPO calculation.

        // This property finds all positions opened by this bot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label used by the bot.
            }
        }

        // This method is called when the bot starts and is used for initialization.
        protected override void OnStart()
        {
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the DPO indicator with the specified parameters.
            _detrendedPriceOscillator = Indicators.DetrendedPriceOscillator(Bars.ClosePrices, Periods, MAType);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the DPO crosses above zero, execute a buy trade.
            if (_detrendedPriceOscillator.Result.Last(0) > 0 && _detrendedPriceOscillator.Result.Last(1) <= 0)
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }
            
            // If the DPO crosses below zero, execute a sell trade.            
            else if (_detrendedPriceOscillator.Result.Last(0) < 0 && _detrendedPriceOscillator.Result.Last(1) >= 0)
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
