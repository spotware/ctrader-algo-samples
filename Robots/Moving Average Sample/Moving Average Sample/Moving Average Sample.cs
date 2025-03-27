// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses two Moving Averages (Fast and Slow) to generate buy and sell signals based on 
//    crossovers. When the Fast MA crosses above the Slow MA, a buy position is opened, and when 
//    the Fast MA crosses below the Slow MA, a sell position is opened.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class MovingAverageSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store the volume in units based on the specified lot size.

        private MovingAverage _fastMa;  // Stores the Fast Moving Average indicator.

        private MovingAverage _slowMa;  // Store the Slow Moving Average indicator.


        // Input parameters for the Fast Moving Average.
        [Parameter("Source", Group = "Fast MA")]
        public DataSeries FastMaSource { get; set; }  // Data source for the Fast MA.

        [Parameter("Period", DefaultValue = 9, Group = "Fast MA")]
        public int FastMaPeriod { get; set; }  // Period for the Fast MA, defaulting to 9.

        [Parameter("Type", DefaultValue = MovingAverageType.Exponential, Group = "Fast MA")]
        public MovingAverageType FastMaType { get; set; }  // Type of the Fast MA, defaulting to Exponential.


        // Input parameters for the Slow Moving Average.
        [Parameter("Source", Group = "Slow MA")]
        public DataSeries SlowMaSource { get; set; }  // Data source for the Slow MA.

        [Parameter("Period", DefaultValue = 20, Group = "Slow MA")]
        public int SlowMaPeriod { get; set; }  // Period for the Slow MA, defaulting to 20.

        [Parameter("Type", DefaultValue = MovingAverageType.Exponential, Group = "Slow MA")]
        public MovingAverageType SlowMaType { get; set; }  // Type of the Slow MA, defaulting to Exponential.


        // Input parameters for trade settings.
        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "MovingAverageSample", Group = "Trade")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

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

            // Initialise the Fast and Slow Moving Averages.
            _fastMa = Indicators.MovingAverage(FastMaSource, FastMaPeriod, FastMaType);
            _slowMa = Indicators.MovingAverage(SlowMaSource, SlowMaPeriod, SlowMaType);

            // Set the line colors for the Fast and Slow MAs.
            _fastMa.Result.Line.Color = Color.Blue;
            _slowMa.Result.Line.Color = Color.Red;
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the Fast MA crosses above the Slow MA, it's considered a buy signal.
            if (_fastMa.Result.HasCrossedAbove(_slowMa.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // If the Fast MA crosses below the Slow MA, it's considered a sell signal.
            else if (_fastMa.Result.HasCrossedBelow(_slowMa.Result, 0))
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
