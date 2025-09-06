// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot utilizes the On-Balance Volume (OBV) indicator in combination with a Simple Moving 
//    Average (SMA) to trigger buy and sell orders. A buy order is placed when OBV crosses above 
//    the SMA, and a sell order is placed when OBV crosses below the SMA.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class OnBalanceVolumeSample : Robot
    {
        // Private fields for storing indicators and trade volume.
        private double _volumeInUnits;  // Store the volume in units based on the specified lot size.

        private OnBalanceVolume _onBalanceVolume;  // Store the On-Balance Volume indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Stores the Simple Moving Average.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "OnBalanceVolumeSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Periods", DefaultValue = 9, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }  // Period for the Simple Moving Average, defaulting to 9.

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

            // Initialise the On-Balance Volume and Simple Moving Average.
            _onBalanceVolume = Indicators.OnBalanceVolume(Bars.ClosePrices);
            _simpleMovingAverage = Indicators.SimpleMovingAverage(_onBalanceVolume.Result, PeriodsSimpleMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the OBV crosses above the SMA, it's considered a buy signal.
            if (_onBalanceVolume.Result.HasCrossedAbove(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // If the OBV crosses below the SMA, it's considered a sell signal.
            else if (_onBalanceVolume.Result.HasCrossedBelow(_simpleMovingAverage.Result, 0))
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
