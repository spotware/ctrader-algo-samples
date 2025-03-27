// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot implements a trading strategy that uses the Relative Strength Index (RSI) indicator.
//    It opens trades when the RSI crosses predefined upper or lower levels and closes positions in
//    the opposite direction when thresholds are breached.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as AccessRights and its ability to add indicators.
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class RelativeStrengthIndexSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private RelativeStrengthIndex _relativeStrengthIndex;  // Store the Relative Strength Index indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "RelativeStrengthIndexSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source", Group = "Relative Strength Index")]
        public DataSeries Source { get; set; }  // Data source for the Relative Strength Index.

        [Parameter(DefaultValue = 14, Group = "Relative Strength Index", MinValue = 1)]
        public int Periods { get; set; }  // The RSI period, defaulting to 14 periods.

        [Parameter("Down level", DefaultValue = 30, Group = "Relative Strength Index", MinValue = 1, MaxValue = 50)]
        public int DownValue { get; set; }  // The lower RSI threshold, defaulting to 30.

        [Parameter("Up level", DefaultValue = 70, Group = "Relative Strength Index", MinValue = 50, MaxValue = 100)]
        public int UpValue { get; set; }  // The upper RSI threshold, defaulting to 70.

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

            // Initialise the RSI indicator using the defined source and period.
            _relativeStrengthIndex = Indicators.RelativeStrengthIndex(Source, Periods);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Checks if the RSI value has crossed above the upper threshold. 
            // The condition ensures the crossover occurred between the previous and current bar.
            if (_relativeStrengthIndex.Result.Last(0) > UpValue && _relativeStrengthIndex.Result.Last(1) <= UpValue)
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Checks if the RSI value has crossed below the lower threshold.
            // The condition ensures the crossover occurred between the previous and current bar.
            else if (_relativeStrengthIndex.Result.Last(0) < DownValue && _relativeStrengthIndex.Result.Last(1) >= DownValue)
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
