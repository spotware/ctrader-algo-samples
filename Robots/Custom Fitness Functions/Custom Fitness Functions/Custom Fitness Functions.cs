// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This example is based on the "Sample Trend cBot". The "Sample Trend cBot" will buy when fast period moving average crosses the slow period moving average and sell when
//    the fast period moving average crosses the slow period moving average. The orders are closed when an opposite signal
//    is generated. There can only by one buy or sell order at any time. The bot also includes a custom fitness function for optimisation.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;
using System.Linq;

namespace cAlgo
{
    // Define the cBot's configuration, including time zone and access rights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CustomFitnessFunctions : Robot
    {
        // Define input parameters for the cBot.
        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }  // Trade volume in lots, with a default of 0.01 lots.

        [Parameter("MA Type", Group = "Moving Average")]
        public MovingAverageType MAType { get; set; }  // Type of moving average to use (e.g., SMA, EMA).

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceSeries { get; set; }  // Data series to calculate the moving average on.

        [Parameter("Slow Periods", Group = "Moving Average", DefaultValue = 10)]
        public int SlowPeriods { get; set; }  // Number of periods for the slow moving average, with a default of 10 periods.

        [Parameter("Fast Periods", Group = "Moving Average", DefaultValue = 5)]
        public int FastPeriods { get; set; }  // Number of periods for the fast moving average, with a default of 5 periods.

        private MovingAverage slowMa;  // Slow moving average instance.
        private MovingAverage fastMa;  // Fast moving average instance.
        private const string label = "Sample Trend cBot";  // Label for identifying positions by the bot.

        // This method is called when the bot starts and is used for initialisation.
        protected override void OnStart()
        {
            fastMa = Indicators.MovingAverage(SourceSeries, FastPeriods, MAType);  // Initialise fast moving average.
            slowMa = Indicators.MovingAverage(SourceSeries, SlowPeriods, MAType);  // Initialise slow moving average.
        }

        // This method is called on each tick update.
        protected override void OnTick()
        {
            var longPosition = Positions.Find(label, SymbolName, TradeType.Buy);  // Check for existing buy position.
            var shortPosition = Positions.Find(label, SymbolName, TradeType.Sell);  // Check for existing sell position.

            var currentSlowMa = slowMa.Result.Last(0);  // Current value of slow moving average.
            var currentFastMa = fastMa.Result.Last(0);  // Current value of fast moving average.
            var previousSlowMa = slowMa.Result.Last(1);  // Previous value of slow moving average.
            var previousFastMa = fastMa.Result.Last(1);  // Previous value of fast moving average.

            // Buy condition: fast MA crosses above slow MA; close any existing sell position.
            if (previousSlowMa > previousFastMa && currentSlowMa <= currentFastMa && longPosition == null)
            {
                if (shortPosition != null)
                    ClosePosition(shortPosition);  // Close any existing sell position.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, VolumeInUnits, label);  // Open a new buy market order.
            }

            // Sell condition: fast MA crosses below slow MA; close any existing buy position.
            else if (previousSlowMa < previousFastMa && currentSlowMa >= currentFastMa && shortPosition == null)
            {
                if (longPosition != null)
                    ClosePosition(longPosition);  // Close any existing buy position.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, VolumeInUnits, label);  // Open a new sell market order.
            }
        }

        // Calculate trading volume in units based on the specified lot quantity.
        private double VolumeInUnits
        {
            get { return Symbol.QuantityToVolumeInUnits(Quantity); }
        }

        // Custom fitness function for optimizing the cBot's performance.
        protected override double GetFitness(GetFitnessArgs args)
        {
            // Checks trade count and drawdown percentage to calculate a custom fitness score.
            if (args.TotalTrades > 20 && args.MaxEquityDrawdownPercentages < 50)
            {
                return Math.Pow(args.WinningTrades + 1, 2) / (args.LosingTrades + 1);
            }
            else
            {
                return double.MinValue;  // Penalize cases that don't meet the criteria.
            }
        }
    }
}
