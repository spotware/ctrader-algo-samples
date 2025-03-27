// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Ichimoku Kinko Hyo indicator to trade trend reversals. It opens a buy position 
//    when the Tenkan Sen crosses above the Kijun Sen, and a sell position when the Tenkan Sen crosses 
//    below the Kijun Sen.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.  
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class IchimokuKinkoHyoSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private IchimokuKinkoHyo _ichimokuKinkoHyo; // Store the Ichimoku Kinko Hyo indicator. 

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "IchimokuKinkoHyoSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Tenkan Sen Periods", DefaultValue = 9, Group = "IchimokuKinkoHyo", MinValue = 1)]
        public int TenkanSenPeriods { get; set; }  // Tenkan Sen period for Ichimoku, default is 9.

        [Parameter("Kijun Sen Periods", DefaultValue = 26, Group = "IchimokuKinkoHyo", MinValue = 1)]
        public int KijunSenPeriods { get; set; }  // Kijun Sen period for Ichimoku, default is 26.

        [Parameter("Senkou Span B Periods", DefaultValue = 52, Group = "IchimokuKinkoHyo", MinValue = 1)]
        public int SenkouSpanBPeriods { get; set; }  // Senkou Span B period for Ichimoku, default is 52.

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

            // Initialise the Ichimoku Kinko Hyo indicator with the specified periods.
            _ichimokuKinkoHyo = Indicators.IchimokuKinkoHyo(TenkanSenPeriods, KijunSenPeriods, SenkouSpanBPeriods);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Check if the current close price is above the Senkou Span B (indicating a buy signal).
            if (Bars.ClosePrices.Last(0) > _ichimokuKinkoHyo.SenkouSpanB.Last(0))
            {
                ClosePositions(TradeType.Sell);  // Close any existing sell positions.

                // Check if Tenkan Sen crosses above Kijun Sen (buy signal).
                if (_ichimokuKinkoHyo.TenkanSen.Last(0) > _ichimokuKinkoHyo.KijunSen.Last(0) && _ichimokuKinkoHyo.TenkanSen.Last(1) <= _ichimokuKinkoHyo.KijunSen.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
                }
            }

            // Check if the current close price is below Senkou Span A (indicating a sell signal).
            else if (Bars.ClosePrices.Last(0) < _ichimokuKinkoHyo.SenkouSpanA.Last(0))
            {
                ClosePositions(TradeType.Buy);  // Close any existing buy positions.

                // Check if Tenkan Sen crosses below Kijun Sen (sell signal).
                if (_ichimokuKinkoHyo.TenkanSen.Last(0) < _ichimokuKinkoHyo.KijunSen.Last(0) && _ichimokuKinkoHyo.TenkanSen.Last(1) >= _ichimokuKinkoHyo.KijunSen.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
                }
            }
        }

        // Method to close positions based on the trade type (buy or sell).
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
