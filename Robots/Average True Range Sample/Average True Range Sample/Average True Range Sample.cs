// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Average True Range (ATR) indicator for volatility-based trade management. 
//    A buy position opens on a bullish candlestick reversal, while a sell opens on a bearish reversal. 
//    ATR-derived stop-loss and take-profit levels are dynamically set based on volatility.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.    
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class AverageTrueRangeSample : Robot
    {
        // Private fields for storing the indicator and calculated volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private AverageTrueRange _averageTrueRange;  // Store the ATR indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01.

        [Parameter("Label", DefaultValue = "AverageTrueRangeSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter(DefaultValue = 14, Group = "Average True Range", MinValue = 1)]
        public int Periods { get; set; }  // ATR calculation period, defaulting to 14.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Average True Range")]
        public MovingAverageType MAType { get; set; }  // Moving average type for the ATR, defaulting to Simple.

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

            // Initialise the ATR indicator with the specified period and moving average type.
            _averageTrueRange = Indicators.AverageTrueRange(Periods, MAType);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if a bullish candlestick reversal has occurred.
            if (Bars.ClosePrices.Last(0) > Bars.OpenPrices.Last(0) && Bars.ClosePrices.Last(1) < Bars.OpenPrices.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteOrder(TradeType.Buy);  // Open a new buy order.
            }

            // Check if a bearish candlestick reversal has occurred.
            else if (Bars.ClosePrices.Last(0) < Bars.OpenPrices.Last(0) && Bars.ClosePrices.Last(1) > Bars.OpenPrices.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteOrder(TradeType.Sell);  // Open a new sell order.
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

        // This method executes a market order based on trade type, with ATR-based stop loss and take profit.
        private void ExecuteOrder(TradeType tradeType)
        {
            // Calculate the ATR in pips, considering symbol precision and tick size.
            var atrInPips = _averageTrueRange.Result.Last(0) * (Symbol.TickSize / Symbol.PipSize * Math.Pow(10, Symbol.Digits));

            // Set the stop-loss at twice the ATR and take-profit at four times the ATR.
            var stopLossInPips = atrInPips * 2;
            var takeProfitInPips = stopLossInPips * 2;

            // Execute a market order with the calculated stop-loss and take-profit.
            ExecuteMarketOrder(tradeType, SymbolName, _volumeInUnits, Label, stopLossInPips, takeProfitInPips);
        }
    }
}
