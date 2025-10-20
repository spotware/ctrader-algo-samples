// ----------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you want to modify this file, please use the "Duplicate" functionality to make a copy.  
//
//    The Sample Trend cBot opens buy orders when the fast period moving average crosses above the slow   
//    period moving average and sell orders when the fast period moving average crosses below the slow  
//    period moving average. The orders are closed when an opposite signal is generated. There can only
//    be one buy or sell order at any time.
//
// ----------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class SampleTrendcBot : Robot
    {
        // Define input parameters for the cBot.
        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }  // Trade quantity in lots.

        [Parameter("MA Type", Group = "Moving Average")]
        public MovingAverageType MAType { get; set; }  // Type of moving average. 

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceSeries { get; set; }  // Data series to calculate the moving average.

        [Parameter("Slow Periods", Group = "Moving Average", DefaultValue = 10)]
        public int SlowPeriods { get; set; }  // Number of periods for the slow moving average.

        [Parameter("Fast Periods", Group = "Moving Average", DefaultValue = 5)]
        public int FastPeriods { get; set; }  // Number of periods for the fast moving average.

        // Private fields for the moving averages and the order label.
        private MovingAverage _slowMa;  // Slow moving average.
        private MovingAverage _fastMa;  // Fast moving average.
        private const string Label = "Sample Trend cBot";  // Label to identify trades made by this cBot.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Initialise the fast and slow moving averages with the specified periods and type.
            _fastMa = Indicators.MovingAverage(SourceSeries, FastPeriods, MAType);
            _slowMa = Indicators.MovingAverage(SourceSeries, SlowPeriods, MAType);
        }

        // This method is called on every price tick.
        protected override void OnTick()
        {
            // Find any open buy or sell positions with the specified label and symbol.
            var longPosition = Positions.Find(Label, SymbolName, TradeType.Buy);
            var shortPosition = Positions.Find(Label, SymbolName, TradeType.Sell);

            // Get the current and previous values of the fast and slow moving averages.
            var currentSlowMa = _slowMa.Result.Last(0);  // Current value of the slow moving average.
            var currentFastMa = _fastMa.Result.Last(0);  // Current value of the fast moving average.
            var previousSlowMa = _slowMa.Result.Last(1);  // Previous value of the slow moving average.
            var previousFastMa = _fastMa.Result.Last(1);  // Previous value of the fast moving average.

            // Buy condition - when the slow MA crosses below the fast MA and no long position exists.
            if (previousSlowMa > previousFastMa && currentSlowMa <= currentFastMa && longPosition == null)
            {
                if (shortPosition != null)  // Close any existing short position.
                    ClosePosition(shortPosition);

                // Open a new buy order.
                ExecuteMarketOrder(TradeType.Buy, SymbolName, VolumeInUnits, Label);
            }
            // Sell condition - when the slow MA crosses above the fast MA and no short position exists.
            else if (previousSlowMa < previousFastMa && currentSlowMa >= currentFastMa && shortPosition == null)
            {
                if (longPosition != null)  // Close any existing long position.
                    ClosePosition(longPosition);

                // Open a new sell order.
                ExecuteMarketOrder(TradeType.Sell, SymbolName, VolumeInUnits, Label);
            }
        }

        // Property to calculate the trade volume in units based on the quantity in lots.
        private double VolumeInUnits
        {
            // Convert the trade quantity in lots to volume in units.
            get { return Symbol.QuantityToVolumeInUnits(Quantity); } 
        }
    }
}
