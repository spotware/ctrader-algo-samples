// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Historical Volatility and Simple Moving Average indicators to manage buy and sell 
//    orders based on market conditions. The bot will open positions when the price crosses the Simple 
//    Moving Average after volatility reaches a specified threshold. It also uses a historical volatility 
//    indicator for additional market insight, with configurable stop loss and take profit values.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.  
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class HistoricalVolatilitySample : Robot
    {
        // Private fields for storing indicators and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private HistoricalVolatility _historicalVolatility;  // Store the Historical Volatility indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "HistoricalVolatilitySample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Periods", DefaultValue = 20, Group = "Historical Volatility", MinValue = 1)]
        public int Periods { get; set; }  // Number of periods used in the Historical Volatility indicator, with a default of 20 periods.

        [Parameter("Bar History", DefaultValue = 252, Group = "Historical Volatility")]
        public int BarHistory { get; set; }  // Number of bars to look back for the Historical Volatility indicator, with a default of 252 periods.

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
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Historical Volatility indicator.
            _historicalVolatility = Indicators.HistoricalVolatility(Bars.ClosePrices, Periods, BarHistory);

            // Initialise the Simple Moving Average indicator.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 9);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Checks if the Historical Volatility is below its maximum in the last 14 periods.            
            if (_historicalVolatility.Result.Last(0) < _historicalVolatility.Result.Maximum(14)) return;

            // If the price crosses above the Simple Moving Average and the previous bar was below the SMA, open a buy order.
            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) < _simpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any existing sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }
            
            // If the price crosses below the Simple Moving Average and the previous bar was above the SMA, open a sell order.
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) > _simpleMovingAverage.Result.Last(1))
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
