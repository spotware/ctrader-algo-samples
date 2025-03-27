// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Linear Regression Intercept indicator to trade based on trend direction changes. 
//    It opens a buy position when the current close price crosses above the Linear Regression Intercept, 
//    and a sell position when the current close price crosses below the Linear Regression Intercept.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class LinearRegressionInterceptSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private LinearRegressionIntercept _linearRegressionIntercept;  // Store the Linear Regression Intercept indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "LinearRegressionInterceptSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Source", Group = "Linear Regression Forecast")]
        public DataSeries Source { get; set; }  // Source data for Linear Regression Intercept.

        [Parameter("Periods", DefaultValue = 9, Group = "Linear Regression Forecast", MinValue = 0)]
        public int Periods { get; set; }  // Number of periods, with a default of 9 periods.

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

            // Initialise the Linear Regression Intercept indicator with the specified period.
            _linearRegressionIntercept = Indicators.LinearRegressionIntercept(Source, Periods);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the current close price crosses above the Linear Regression Intercept, execute a buy trade.
            if (Bars.ClosePrices.Last(0) > _linearRegressionIntercept.Result.Last(0) && Bars.ClosePrices.Last(1) <= _linearRegressionIntercept.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips); // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // If the current close price crosses below the Linear Regression Intercept, execute a sell trade.
            else if (Bars.ClosePrices.Last(0) < _linearRegressionIntercept.Result.Last(0) && Bars.ClosePrices.Last(1) >= _linearRegressionIntercept.Result.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips); // Open a sell market order with the specified volume, stop loss and take profit.
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
