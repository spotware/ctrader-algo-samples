// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Money Flow Index (MFI) to generate buy and sell signals. It opens a sell position 
//    when the MFI crosses above a certain threshold (Level Up), and opens a buy position when the MFI crosses 
//    below a lower threshold (Level Down).
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class MoneyFlowIndexSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store the volume in units based on the specified lot size.

        private MoneyFlowIndex _moneyFlowIndex;  // Store the Money Flow Index (MFI) indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "MoneyFlowIndexSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Periods", DefaultValue = 14, Group = "Money Flow Index", MinValue = 2)]
        public int Periods { get; set; }  // Periods for the Money Flow Index, defaulting to 14.

        [Parameter("Level Up", DefaultValue = 80, Group = "Money Flow Index", MinValue = 50, MaxValue = 100)]
        public int LevelUp { get; set; }  // Upper level threshold for the MFI, defaulting to 80.

        [Parameter("Level Down", DefaultValue = 20, Group = "Money Flow Index", MinValue = 0, MaxValue = 50)]
        public int LevelDown { get; set; }  // Lower level threshold for the MFI, defaulting to 20.

        // This property retrieves all positions opened by this bot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label as this bot.
            }
        }

        // This method is called when the bot starts and is used for initialization.
        protected override void OnStart()
        {
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Money Flow Index (MFI) indicator.
            _moneyFlowIndex = Indicators.MoneyFlowIndex(Periods);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the MFI is above the upper threshold, it's considered overbought, triggering a sell signal.
            if (_moneyFlowIndex.Result.Last(0) > LevelUp)
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                // Open a sell order if no positions are open.
                if (BotPositions.Length == 0)
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
                }
            }

            // If the MFI is below the lower threshold, it's considered oversold, triggering a buy signal.
            else if (_moneyFlowIndex.Result.Last(0) < LevelDown)
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                // Open a buy order if no positions are open.
                if (BotPositions.Length == 0)
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
                }
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
