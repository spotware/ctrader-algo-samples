// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This sample cBot trades based on the crossover of two Exponential Moving Averages (EMA).
//    When the fast EMA crosses above the slow EMA, it buys. When the fast EMA crosses below the slow EMA, it sells.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.  
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class ExponentialMovingAverageSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private ExponentialMovingAverage _fastExponentialMovingAverage;  // Store the first (fast) EMA.

        private ExponentialMovingAverage _slowExponentialMovingAverage;  // Store the slow EMA.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "ExponentialMovingAverageSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        // Parameters for the fast EMA.
        [Parameter("Periods", DefaultValue = 9, Group = "Exponential Moving Average 1", MinValue = 0)]
        public int PeriodsFirst { get; set; }  // Number of periods for the fast EMA, default is 9.

        [Parameter("Source", Group = "Exponential Moving Average 1")]
        public DataSeries SourceFirst { get; set; }  // Data series used for the fast EMA.

        // Parameters for the slow EMA.
        [Parameter("Periods", DefaultValue = 20, Group = "Exponential Moving Average 2", MinValue = 0)]
        public int PeriodsSecond { get; set; }  // Number of periods for the slow EMA, default is 20.

        [Parameter("Source", Group = "Exponential Moving Average 2")]
        public DataSeries SourceSecond { get; set; }  // Data series used for the slow EMA.

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

            // Initialise the fast EMA with the specified periods and source.
            _fastExponentialMovingAverage = Indicators.ExponentialMovingAverage(SourceFirst, PeriodsFirst);

            // Set the color of the fast EMA line to blue.
            _fastExponentialMovingAverage.Result.Line.Color = Color.Blue;

            // Initialise the slow EMA with the specified periods and source.
            _slowExponentialMovingAverage = Indicators.ExponentialMovingAverage(SourceSecond, PeriodsSecond);

            // Set the color of the slow EMA line to red.
            _slowExponentialMovingAverage.Result.Line.Color = Color.Red;
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the fast EMA crosses above the slow EMA, execute a buy order.
            if (_fastExponentialMovingAverage.Result.Last(0) > _slowExponentialMovingAverage.Result.Last(0) && _fastExponentialMovingAverage.Result.Last(1) < _slowExponentialMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }
            
            // If the fast EMA crosses below the slow EMA, execute a sell order
            else if (_fastExponentialMovingAverage.Result.Last(0) < _slowExponentialMovingAverage.Result.Last(0) && _fastExponentialMovingAverage.Result.Last(1) > _slowExponentialMovingAverage.Result.Last(1))
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
