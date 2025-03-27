// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This example cBot uses the Ease of Movement indicator to detect price movement and combine it with a 
//    Simple Moving Average (SMA) crossover strategy. The bot opens a buy position when the price crosses above 
//    the SMA and a sell position when the price crosses below the SMA.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.  
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class EaseOfMovementSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private EaseOfMovement _easeOfMovement;  // Store the Ease of Movement indicator.

        private MovingAverage _simpleMovingAverage;  // Store the Simple Moving Average indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "EaseOfMovementSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Periods", DefaultValue = 14, Group = "Ease Of Movement", MinValue = 1)]
        public int Periods { get; set; }  // Number of periods for the Ease of Movement, defaulting to 14.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Ease Of Movement")]
        public MovingAverageType MAType { get; set; }  // Type of moving average used for the crossover strategy.

        // This property finds all positions opened by this bot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label used by the bot.
            }
        }

        // This method is called when the cBot starts and is used for initialization.
        protected override void OnStart()
        {
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Ease of Movement indicator with the specified period and MA type.
            _easeOfMovement = Indicators.EaseOfMovement(Periods, MAType);

            // Initialise the Simple Moving Average indicator with a period of 9.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 9);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the Ease of Movement is above a threshold, check the price crossover.
            if (_easeOfMovement.Result.Last(0) > (Symbol.TickSize * 0.05))
            {
                // If the price crosses above the SMA, execute a buy trade.
                if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) < _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
                }

                // If the price crosses below the SMA, execute a sell trade.
                else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) > _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss, and take profit.
                }
            }

            // If Ease of Movement is low, close all open positions.
            else
            {
                ClosePositions();
            }
        }

        // This method closes all positions opened by the bot.
        private void ClosePositions()
        {
            foreach (var position in BotPositions)
            {
                ClosePosition(position);  // Close the position.
            }
        }
    }
}
