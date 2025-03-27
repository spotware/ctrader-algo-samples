// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Ultimate Oscillator to trade based on overbought and oversold conditions. 
//    It opens a sell position when the oscillator crosses below the upper level and a buy position 
//    when it crosses above the lower level.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class UltimateOscillatorSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private UltimateOscillator _ultimateOscillator;  // Store the Ultimate Oscillator indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "UltimateOscillatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Cycle 1", DefaultValue = 7, Group = "Ultimate Oscillator", MinValue = 1)]
        public int Cycle1 { get; set; }  // First cycle length of the oscillator, default is 7.

        [Parameter("Cycle 2", DefaultValue = 14, Group = "Ultimate Oscillator", MinValue = 1)]
        public int Cycle2 { get; set; }  // Second cycle length of the oscillator, default is 14.

        [Parameter("Cycle 3", DefaultValue = 28, Group = "Ultimate Oscillator", MinValue = 1)]
        public int Cycle3 { get; set; }  // Third cycle length of the oscillator, default is 28.

        [Parameter("Down level", DefaultValue = 30, Group = "Stochastic Oscillator", MinValue = 1, MaxValue = 50)]
        public int DownValue { get; set; }  // Lower level of the oscillator, default is 30.

        [Parameter("Up level", DefaultValue = 70, Group = "Stochastic Oscillator", MinValue = 50, MaxValue = 100)]
        public int UpValue { get; set; }  // Upper level of the oscillator, default is 70.

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

            // Initialise the Ultimate Oscillator with the specified cycles.
            _ultimateOscillator = Indicators.UltimateOscillator(Cycle1, Cycle2, Cycle3);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.        
        protected override void OnBarClosed()
        {
            // Check if the oscillator crosses below the upper level.
            if (_ultimateOscillator.Result.Last(0) > UpValue && _ultimateOscillator.Result.Last(1) < UpValue)
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to sell with the specified volume, stop loss and take profit.
            }
            
            // Check if the oscillator crosses above the lower level.
            else if (_ultimateOscillator.Result.Last(0) < DownValue && _ultimateOscillator.Result.Last(1) > DownValue)
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
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
