// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Directional Movement System (DMS) to trade based on trend strength and direction. 
//    It opens a buy position when the +DI line crosses above the -DI line and a sell position when the 
//    -DI line crosses above the +DI line, provided the ADX value is above a specified level. 
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class DirectionalMovementSystemSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private DirectionalMovementSystem _directionalMovementSystem;  // Store the DMS indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, default of 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, default of 10 pips.

        [Parameter("Label", DefaultValue = "DirectionalMovementSystemSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter(DefaultValue = 14, Group = "Directional Movement System", MinValue = 1, MaxValue = 10000)]
        public int Periods { get; set; }  // Number of periods for the DMS calculation, default of 14 periods.

        [Parameter("ADX Level", DefaultValue = 25, Group = "Directional Movement System")]
        public int ADXLevel { get; set; }  // ADX level threshold, default is 25.

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

            // Initialise the DMS indicator with the specified period.
            _directionalMovementSystem = Indicators.DirectionalMovementSystem(Periods);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If ADX is below the specified level, avoid trading.
            if (_directionalMovementSystem.ADX.Last(0) < ADXLevel) return;

            // If +DI crosses above -DI, execute a buy trade.
            if (_directionalMovementSystem.DIPlus.Last(0) > _directionalMovementSystem.DIMinus.Last(0) && _directionalMovementSystem.DIPlus.Last(1) <= _directionalMovementSystem.DIMinus.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // If -DI crosses above +DI, execute a sell trade.
            else if (_directionalMovementSystem.DIPlus.Last(0) < _directionalMovementSystem.DIMinus.Last(0) && _directionalMovementSystem.DIPlus.Last(1) >= _directionalMovementSystem.DIMinus.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

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
