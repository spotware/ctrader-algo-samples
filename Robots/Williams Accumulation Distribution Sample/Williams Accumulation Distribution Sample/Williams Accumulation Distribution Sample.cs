// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Williams Accumulation/Distribution (WAD) indicator and a Simple Moving 
//    Average (SMA) to identify potential buy and sell signals. Correlation analysis is applied to 
//    evaluate the relationship between WAD and price movements. Signals are filtered out when 
//    correlation exceeds a threshold, focusing on uncorrelated data for trade execution.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;
using System.Linq;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class WilliamsAccumulationDistributionSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private WilliamsAccumulationDistribution _williamsAccumulationDistribution;  // Store the WAD indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average for trade decisions.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, Group = "Trade", MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, Group = "Trade", MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "WilliamsAccumulationDistributionSample", Group = "Trade")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceMovingAverage { get; set; }  // Data series for SMA calculation.

        [Parameter("Periods Moving Average", DefaultValue = 14, Group = "Simple Moving Average", MinValue = 2)]
        public int PeriodsMovingAverage { get; set; }  // Number of periods for the SMA, default is 14.

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

            // Initialise the WAD indicator.
            _williamsAccumulationDistribution = Indicators.WilliamsAccumulationDistribution();

            // Initialise the SMA with the given data source and period.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceMovingAverage, PeriodsMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            var correlation = GetCorrelation(14);  // Calculate the correlation over the last 14 periods.

            // Skip trade execution if the correlation is too high.
            if (correlation > 0.85) return;

            // Check if the current close price is above the SMA to trigger a sell signal.
            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to sell with the specified volume, stop loss and take profit.
            }
            
            // Check if the current close price is below the SMA to trigger a buy signal.
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
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

        // This method calculates the correlation between WAD and close prices over a specified period.
        private double GetCorrelation(int period)
        {
            var x = _williamsAccumulationDistribution.Result.Skip(_williamsAccumulationDistribution.Result.Count - period).ToArray();  // Extract WAD values for the period.
            var y = Bars.ClosePrices.Skip(Bars.ClosePrices.Count - period).ToArray();  // Extract close prices for the period.

            if (!x.Any() || !y.Any())
            {
                return double.NaN;  // Return NaN if data is insufficient.
            }

            // Calculate sums and squared sums for both datasets.
            var xSum = x.Sum();
            var ySum = y.Sum();

            var xSumSquared = Math.Pow(xSum, 2);
            var ySumSquared = Math.Pow(ySum, 2);

            var xSquaredSum = x.Select(value => Math.Pow(value, 2)).Sum();
            var ySquaredSum = y.Select(value => Math.Pow(value, 2)).Sum();

            var xAndyProductSum = x.Zip(y, (value1, value2) => value1 * value2).Sum();  // Calculate the sum of the product of corresponding values in both datasets.

            double n = x.Count();  // Number of data points.

            // Return the correlation coefficient.
            return (n * xAndyProductSum - xSum * ySum) / Math.Sqrt((n * xSquaredSum - xSumSquared) * (n * ySquaredSum - ySumSquared));
        }
    }
}
