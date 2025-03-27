// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot utilises the True Range indicator to execute trades based on price reversals.
//    It identifies bullish or bearish patterns using candlestick close and open values.
//    It sets dynamic stop-loss and take-profit levels based on the True Range value.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class TrueRangeSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private TrueRange _trueRange;  // Store the True Range indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Label", DefaultValue = "TrueRangeSample")]
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

            // Initialise the True Range indicator.
            _trueRange = Indicators.TrueRange();
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check for a bullish reversal pattern.
            if (Bars.ClosePrices.Last(0) > Bars.OpenPrices.Last(0) && Bars.ClosePrices.Last(1) < Bars.OpenPrices.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any existing sell positions.

                ExecuteOrder(TradeType.Buy);  // Execute a buy order.
            }
            
             // Check for a bearish reversal pattern.
            else if (Bars.ClosePrices.Last(0) < Bars.OpenPrices.Last(0) && Bars.ClosePrices.Last(1) > Bars.OpenPrices.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any existing buy positions.

                ExecuteOrder(TradeType.Sell);  // Execute a sell order.
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

        // This method is used to execute a market order with a specified trade type.
        private void ExecuteOrder(TradeType tradeType)
        {
            // Calculate the True Range value in pips using symbol properties.
            var trueRangeInPips = _trueRange.Result.Last(1) * (Symbol.TickSize / Symbol.PipSize * Math.Pow(10, Symbol.Digits));

            var stopLossInPips = trueRangeInPips * 2;  // Set the stop-loss level as twice the True Range value in pips.
            var takeProfitInPips = stopLossInPips * 2;  // Set the take-profit level as twice the stop-loss value.

            // Open a market order with the specified trade type, volume, stop loss and take profit.
            ExecuteMarketOrder(tradeType, SymbolName, _volumeInUnits, Label, stopLossInPips, takeProfitInPips);
        }
    }
}
