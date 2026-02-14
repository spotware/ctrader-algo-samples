// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Price Oscillator to trade based on bullish and bearish crossovers. 
//    It opens a buy position when the oscillator crosses above zero and a sell position when it 
//    crosses below zero.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class PriceOscillatorSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private PriceOscillator _priceOscillator;  // Store the Price Oscillator indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "PriceOscillatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source", Group = "Price Oscillator")]
        public DataSeries Source { get; set; }  // Data series for the Price Oscillator calculation.

        [Parameter("Long Cycle", DefaultValue = 22, Group = "Price Oscillator", MinValue = 1)]
        public int LongCycle { get; set; }  // Number of periods for the long cycle, defaulting to 22.

        [Parameter("Short Cycle", DefaultValue = 14, Group = "Price Oscillator", MinValue = 1)]
        public int ShortCycle { get; set; }  // Number of periods for the short cycle, defaulting to 14.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Price Oscillator")]
        public MovingAverageType MAType { get; set; }  // Moving average type, default is Simple.

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

            // Initialise the Price Oscillator indicator with the specified period.
            _priceOscillator = Indicators.PriceOscillator(Source, LongCycle, ShortCycle, MAType);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // If the Price Oscillator crosses above zero, execute a buy trade.
            if (_priceOscillator.Result.Last(0) > 0 && _priceOscillator.Result.Last(1) <= 0)
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // If the Price Oscillator crosses below zero, execute a sell trade.
            else if (_priceOscillator.Result.Last(0) < 0 && _priceOscillator.Result.Last(1) >= 0)
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

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
