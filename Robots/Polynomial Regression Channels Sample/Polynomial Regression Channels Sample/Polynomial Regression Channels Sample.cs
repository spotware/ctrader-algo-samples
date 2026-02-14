// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot is an example of using Polynomial Regression Channels with the cTrader Algo API.
//    The bot uses these channels to detect potential trend reversals, closing the opposite positions 
//    and opening new ones based on price action relative to the regression channels.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class PolynomialRegressionChannelsSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private PolynomialRegressionChannels _polynomialRegressionChannels;  // Store the Polynomial Regression Channels indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Label", DefaultValue = "PolynomialRegressionChannelsSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        // Parameters for configuring the Polynomial Regression Channels.
        [Parameter("Source", Group = "Polynomial Regression Channels")]
        public DataSeries Source { get; set; }  // Parameter to define the source data series (e.g., closing prices).

        [Parameter(DefaultValue = 3.0, Group = "Polynomial Regression Channels", MinValue = 1, MaxValue = 4)]
        public int Degree { get; set; }  // Polynomial degree setting with range between 1 and 4.

        [Parameter(DefaultValue = 120, Group = "Polynomial Regression Channels", MinValue = 1)]
        public int Periods { get; set; }  // Period setting for the Polynomial Regression Channels.

        [Parameter("Standard Deviation", DefaultValue = 1.62, Group = "Polynomial Regression Channels", Step = 0.01)]
        public double StandardDeviation { get; set; }  // Standard deviation parameter for the first channel.

        [Parameter("Standard Deviation 2", DefaultValue = 2, Group = "Polynomial Regression Channels", Step = 0.01)]
        public double StandardDeviation2 { get; set; }  // Standard deviation parameter for the second channel.

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

            // Instantiates the Polynomial Regression Channels indicator with parameters.
            _polynomialRegressionChannels = Indicators.PolynomialRegressionChannels(Degree, Periods, StandardDeviation, StandardDeviation2);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the low price of the current bar crosses below the lower regression channel.
            if (Bars.LowPrices.Last(0) <= _polynomialRegressionChannels.Sql.Last(0) && Bars.LowPrices.Last(1) > _polynomialRegressionChannels.Sql.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any existing sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);  // Open a market order to buy.
            }
            
            // Check if the high price of the current bar crosses above the upper regression channel.
            else if (Bars.HighPrices.Last(0) >= _polynomialRegressionChannels.Sqh.Last(0) && Bars.HighPrices.Last(1) < _polynomialRegressionChannels.Sqh.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any existing buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label);  // Open a market order to sell.
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
