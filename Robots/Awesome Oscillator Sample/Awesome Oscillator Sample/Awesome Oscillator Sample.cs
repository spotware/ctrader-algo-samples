// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Awesome Oscillator (AO) to identify momentum and trend direction. 
//    The cBot opens a buy position when the AO crosses above zero, indicating a potential 
//    upward momentum shift. It opens a sell position when the AO crosses below zero, 
//    suggesting a potential downward momentum shift.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.    
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class AwesomeOscillatorSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private AwesomeOscillator _awesomeOscillator;  // Store the AO indicator.

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "AwesomeOscillatorSample")]
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

            // Initialise the AO indicator with the specified period and moving average type.
            _awesomeOscillator = Indicators.AwesomeOscillator();
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Loop through all positions opened by this cBot to check if they meet close conditions.
            foreach (var position in BotPositions)
            {
                // If there's a buy position and AO shows a downtrend (crosses below previous value) or 
                // a sell position and AO shows an uptrend (crosses above previous value), close the position.
                if ((position.TradeType == TradeType.Buy && _awesomeOscillator.Result.Last(0) < _awesomeOscillator.Result.Last(1))
                    || (position.TradeType == TradeType.Sell && _awesomeOscillator.Result.Last(0) > _awesomeOscillator.Result.Last(1)))
                {
                    ClosePosition(position);  // Close the position.
                }
            }

            // Check for AO crossing above zero (bullish signal), triggering a buy order.
            if (_awesomeOscillator.Result.Last(0) > 0 && _awesomeOscillator.Result.Last(1) <= 0)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }

            // Check for AO crossing below zero (bearish signal), triggering a sell order.
            else if (_awesomeOscillator.Result.Last(0) < 0 && _awesomeOscillator.Result.Last(1) >= 0)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to sell with the specified volume, stop loss and take profit.
            }
        }
    }
}
