// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The Accumulative Swing Index Sample cBot uses the Accumulative Swing Index (ASI) indicator to generate 
//    buy signals when it crosses above zero and sell signals when it crosses below zero. Positions are closed 
//    when an opposite signal is generated or by reaching the stop loss/take profit limits. Only one buy or 
//    sell position is maintained at any given time.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.   
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class AccumulativeSwingIndexSample : Robot
    {
        // Private fields for storing the indicator and trade volume.        
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.
        
        private AccumulativeSwingIndex _accumulativeSwingIndex;  // Store the Accumulative Swing Index indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "AccumulativeSwingIndexSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Limit Move Value", DefaultValue = 12, Group = "Accumulative Swing Index", MinValue = 0)]
        public int LimitMoveValue { get; set; }  // ASI limit move value, used to determine the ASI sensitivity.

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

            // Initialise the Accumulative Swing Index with the given limit move value.
            _accumulativeSwingIndex = Indicators.AccumulativeSwingIndex(LimitMoveValue);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check existing positions and manage them based on the ASI indicator signals.           
            foreach (var position in BotPositions)
            {
                // Close Buy positions if the ASI shows a downward signal.
                // Close Sell positions if the ASI shows an upward signal.                
                if ((position.TradeType == TradeType.Buy && _accumulativeSwingIndex.Result.Last(0) < _accumulativeSwingIndex.Result.Last(1))
                    || (position.TradeType == TradeType.Sell && _accumulativeSwingIndex.Result.Last(0) > _accumulativeSwingIndex.Result.Last(1)))
                {
                    ClosePosition(position);  // Close the position when the opposite signal appears.
                }
            }

            // Evaluate conditions to open new positions based on zero-crossing of the ASI.
            
            // If ASI crosses above zero, a buy signal is generated.
            if (_accumulativeSwingIndex.Result.Last(0) > 0 && _accumulativeSwingIndex.Result.Last(1) <= 0)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // If ASI crosses below zero, a sell signal is generated.           
            else if (_accumulativeSwingIndex.Result.Last(0) < 0 && _accumulativeSwingIndex.Result.Last(1) >= 0)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to sell with the specified volume, stop loss and take profit.
            }
        }
    }
}
