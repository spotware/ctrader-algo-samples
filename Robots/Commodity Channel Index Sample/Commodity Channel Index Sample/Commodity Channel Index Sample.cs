// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This sample demonstrates the use of the Commodity Channel Index (CCI) to execute trades
//    when the CCI crosses over specific threshold values (up and down levels).
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class CommodityChannelIndexSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private CommodityChannelIndex _commodityChannelIndex;  // Store the Commodity Channel Index indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "CommodityChannelIndexSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter(DefaultValue = 20, Group = "Commodity Channel Index", MinValue = 1)]
        public int Periods { get; set; } // Periods for the CCI calculation, with a default of 20 periods.

        [Parameter("Down level", DefaultValue = -100, Group = "Commodity Channel Index")]
        public int DownValue { get; set; }  // The down level threshold for the CCI, defaulting to -100.

        [Parameter("Up level", DefaultValue = 100, Group = "Commodity Channel Index")]
        public int UpValue { get; set; }  // The up level threshold for the CCI, defaulting to 100.

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
            // Convert the specified lot size to volume in units for the trading symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Commodity Channel Index indicator with the specified period.
            _commodityChannelIndex = Indicators.CommodityChannelIndex(Periods);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the CCI crosses above the up level, close any sell positions and place a buy order.
            if (_commodityChannelIndex.Result.Last(0) > UpValue && _commodityChannelIndex.Result.Last(1) <= UpValue)
            {
                ClosePositions(TradeType.Sell);  // Close any existing sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }
            else if (_commodityChannelIndex.Result.Last(0) < DownValue && _commodityChannelIndex.Result.Last(1) >= DownValue)
            {
                ClosePositions(TradeType.Buy);  // Close any existing buy positions.

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
