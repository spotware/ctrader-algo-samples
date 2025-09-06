// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to use two Triangular Moving Averages (TMA) to identify trade opportunities. 
//    A buy trade is executed when the fast TMA crosses above the slow TMA and a sell trade is executed when 
//    the fast TMA crosses below the slow TMA.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class TriangularMovingAverageSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private TriangularMovingAverage _fastTriangularMovingAverage;  // Store the fast Triangular Moving Average.

        private TriangularMovingAverage _slowTriangularMovingAverage;  // Store the slow Triangular Moving Average.

        // Define input parameters for the cBot.
        [Parameter("Source", Group = "Fast MA")]
        public DataSeries FastMaSource { get; set; }  // Data source for the fast Triangular Moving Average.

        [Parameter("Period", DefaultValue = 9, Group = "Fast MA")]
        public int FastMaPeriod { get; set; }  // Period for the fast Triangular Moving Average, default value is 9.

        [Parameter("Source", Group = "Slow MA")]
        public DataSeries SlowMaSource { get; set; }  // Data source for the slow Triangular Moving Average.

        [Parameter("Period", DefaultValue = 20, Group = "Slow MA")]
        public int SlowMaPeriod { get; set; }  // Period for the fast Triangular Moving Average, default value is 20.

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "TriangularMovingAverageSample", Group = "Trade")]
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

            // Initialise the fast and slow Triangular Moving Average indicators with the specified periods.
            _fastTriangularMovingAverage = Indicators.TriangularMovingAverage(FastMaSource, FastMaPeriod);
            _slowTriangularMovingAverage = Indicators.TriangularMovingAverage(SlowMaSource, SlowMaPeriod);

            // Set colors for the moving averages, blue for the fast MA and red for the slow MA.
            _fastTriangularMovingAverage.Result.Line.Color = Color.Blue;
            _slowTriangularMovingAverage.Result.Line.Color = Color.Red;
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the fast TMA crosses above the slow TMA to trigger a buy signal.
            if (_fastTriangularMovingAverage.Result.HasCrossedAbove(_slowTriangularMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any existing sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Check if the fast TMA crosses below the slow TMA to trigger a sell signal.
            else if (_fastTriangularMovingAverage.Result.HasCrossedBelow(_slowTriangularMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Buy);  // Close any existing buy positions.

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
