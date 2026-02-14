// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The Average Directional Movement Index Rating (ADXR) Sample cBot opens buy or sell trades 
//    based on trend strength. When ADXR exceeds a set level, a buy order is placed if DI+ crosses 
//    above DI-, and a sell if DI- crosses above DI+. The cBot manages risk with stop-loss and 
//    take-profit settings and closes opposite positions before opening new ones.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.    
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class AverageDirectionalMovementIndexRatingSample : Robot
    {
        // Private fields for storing the indicator and trade volume.        
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private AverageDirectionalMovementIndexRating _averageDirectionalMovementIndexRating;  // Store the ADXR indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "AverageDirectionalMovementIndexRatingSample")]
        public string Label { get; set; }  // Unique label for identifying orders opened by this cBot.

        [Parameter("Periods", DefaultValue = 14, Group = "Average Directional Movement Index Ratin")]
        public int Periods { get; set; }  // Periods setting for the ADXR indicator, defaulting to 14.

        [Parameter("ADXR Level", DefaultValue = 25, Group = "Average Directional Movement Index Ratin")]
        public int ADXRLevel { get; set; }  // ADXR threshold level for determining if trend is strong enough, defaulting to 25.

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

            // Initialise the ADXR indicator with specified periods.
            _averageDirectionalMovementIndexRating = Indicators.AverageDirectionalMovementIndexRating(20);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // If ADXR level is below threshold, do not take any trades, indicating weak trend.            
            if (_averageDirectionalMovementIndexRating.ADXR.Last(0) < ADXRLevel) return;

            // Check conditions to open a buy or sell position based on DI+ and DI- line crossover.

            // Buy when DI+ crosses above DI-.
            if (_averageDirectionalMovementIndexRating.DIPlus.Last(0) > _averageDirectionalMovementIndexRating.DIMinus.Last(0) && _averageDirectionalMovementIndexRating.DIPlus.Last(1) <= _averageDirectionalMovementIndexRating.DIMinus.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }

            // Sell when DI+ crosses below DI-.
            else if (_averageDirectionalMovementIndexRating.DIPlus.Last(0) < _averageDirectionalMovementIndexRating.DIMinus.Last(0) && _averageDirectionalMovementIndexRating.DIPlus.Last(1) >= _averageDirectionalMovementIndexRating.DIMinus.Last(1))
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
