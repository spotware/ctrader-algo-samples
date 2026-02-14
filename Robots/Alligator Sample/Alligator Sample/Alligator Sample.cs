// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Alligator indicator to make automated trading decisions based on price trends.
//    It enters a buy or sell position when the Alligator’s Lips and Teeth lines cross, indicating 
//    potential trend changes. The cBot will also close any open positions of the opposite type to 
//    prevent conflicting trades.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.   
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class AlligatorSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private Alligator _alligator;  // Store the Alligator indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, with a default of 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "AlligatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Jaws Periods", DefaultValue = 13, Group = "Alligator", MinValue = 1)]
        public int JawsPeriods { get; set; }  // Number of periods for the Jaws line, with a default of 13 periods.

        [Parameter("Jaws Shift", DefaultValue = 18, Group = "Alligator", MinValue = 0, MaxValue = 1000)]
        public int JawsShift { get; set; }  // Shift (offset) for the Jaws line, with a default of 18 bars.

        [Parameter("Teeth Periods", DefaultValue = 8, Group = "Alligator", MinValue = 1)]
        public int TeethPeriods { get; set; }  // Number of periods for calculating the Teeth line, with a default of 8 periods.

        [Parameter("Teeth Shift", DefaultValue = 5, Group = "Alligator", MinValue = 0, MaxValue = 1000)]
        public int TeethShift { get; set; }  // Shift (offset) for the Teeth line, with a default of 5 bars.

        [Parameter("Lips Periods", DefaultValue = 5, Group = "Alligator", MinValue = 1)]
        public int LipsPeriods { get; set; }  // Number of periods for calculating the Lips line, with a default of 5 periods.

        [Parameter("Lips Shift", DefaultValue = 3, Group = "Alligator", MinValue = 0, MaxValue = 1000)]
        public int LipsShift { get; set; }  // Shift (offset) for the Lips line, with a default of 3 bars.

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

            // Initialise the Alligator indicator with the configured parameters.
            _alligator = Indicators.Alligator(JawsPeriods, JawsShift, TeethPeriods, TeethShift, LipsPeriods, LipsShift);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the Lips line has crossed above the Teeth line (indicating a possible buy signal).
            if (_alligator.Lips.Last(0) > _alligator.Teeth.Last(0) && _alligator.Lips.Last(1) <= _alligator.Teeth.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions before buying.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Check if the Lips line has crossed below the Teeth line (indicating a possible sell signal).
            else if (_alligator.Lips.Last(0) < _alligator.Teeth.Last(0) && _alligator.Lips.Last(1) >= _alligator.Teeth.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions before selling.

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
