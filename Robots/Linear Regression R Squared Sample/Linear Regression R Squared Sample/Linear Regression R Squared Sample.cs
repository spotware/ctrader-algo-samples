// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Linear Regression R-Squared indicator combined with two types of moving averages 
//    (Simple and Exponential) to trade. It opens a buy position when the price crosses above the Exponential 
//    Moving Average and the Linear Regression R-Squared is greater than the Simple Moving Average. 
//    It opens a sell position when the price crosses below the Exponential Moving Average with the same condition 
//    on the Linear Regression R-Squared.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class LinearRegressionRSquaredSample : Robot
    {
        // Private fields for storing indicators and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private LinearRegressionRSquared _linearRegressionRSquared;  // Store the Linear Regression R-Squared indicator.
        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average.
        private ExponentialMovingAverage _exponentialMovingAverage;  // Store the Exponential Moving Average.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "LinearRegressionRSquaredSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Source", Group = "Linear Regression")]
        public DataSeries SourceLinearRegression { get; set; }  // Source data for the Linear Regression R-Squared indicator.

        [Parameter("Periods", DefaultValue = 20, Group = "Linear Regression", MinValue = 0)]
        public int PeriodsLinearRegression { get; set; }  // Number of periods for Linear Regression R-Squared, default is 20.

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceSimpleMovingAverage { get; set; }  // Source data for the Simple Moving Average.

        [Parameter("Periods", DefaultValue = 10, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }  // Number of periods for Simple Moving Average, default is 10.

        [Parameter("Source", Group = "Exponential Moving Average")]
        public DataSeries SourceExponentialMovingAverage { get; set; }  // Source data for the Exponential Moving Average.

        [Parameter("Periods", DefaultValue = 20, Group = "Exponential Moving Average", MinValue = 0)]
        public int PeriodsExponentialMovingAverage { get; set; }  // Number of periods for Exponential Moving Average, default is 20.

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

            // Initialise the Linear Regression R-Squared indicator with the specified period.
            _linearRegressionRSquared = Indicators.LinearRegressionRSquared(SourceLinearRegression, PeriodsLinearRegression);

            // Initialise the Simple Moving Average with the Linear Regression R-Squared results.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(_linearRegressionRSquared.Result, PeriodsSimpleMovingAverage);

            // Initialise the Exponential Moving Average with the specified source and period.
            _exponentialMovingAverage = Indicators.ExponentialMovingAverage(SourceExponentialMovingAverage, PeriodsExponentialMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the current close price crosses above the Exponential Moving Average, execute a buy trade.
            if (Bars.ClosePrices.Last(0) > _exponentialMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) <= _exponentialMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                // Execute Buy if the Linear Regression R-Squared is above the Simple Moving Average.
                if (_linearRegressionRSquared.Result.Last(0) > _simpleMovingAverage.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
                }
            }

            // If the current close price crosses below the Exponential Moving Average, execute a sell trade.
            else if (Bars.ClosePrices.Last(0) < _exponentialMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) >= _exponentialMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                // Execute Buy if the Linear Regression R-Squared is above the Simple Moving Average.
                if (_linearRegressionRSquared.Result.Last(0) > _simpleMovingAverage.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
                }
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
