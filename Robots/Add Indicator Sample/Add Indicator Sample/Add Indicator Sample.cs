// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
// 
//    This sample cBot adds two moving averages for trading to a chart. The cBot opens a buy position 
//    when the fast moving average crosses above the slow moving average, and a sell position when the fast 
//    moving average crosses below the slow moving average. Only one buy or sell position is open at a time.
//
//    For a detailed tutorial on creating this cBot, see this video: https://www.youtube.com/watch?v=DUzdEt30OSE
//
// -------------------------------------------------------------------------------------------------


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
        public MovingAverageType MAType { get; set; }  // Type of moving average (e.g., Simple, Exponential).

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceSeries { get; set; }  // Data source for moving averages.

        [Parameter("Slow Periods", Group = "Moving Average", DefaultValue = 10)]
        public int SlowPeriods { get; set; }  // Periods for the slow moving average.

        [Parameter("Fast Periods", Group = "Moving Average", DefaultValue = 5)]
        public int FastPeriods { get; set; }  // Periods for the fast moving average.

        // Private fields for the slow and fast moving averages and labels for identifying trades.
        private MovingAverage slowMa;  // Slow moving average.
        private MovingAverage fastMa;  // Fast moving average.
        private const string label = "Sample Trend cBot";  // Label for tracking cBot positions.

        // Fields for visual chart indicators.
        ChartIndicator _indicator1;
        ChartIndicator _indicator2;

        // This method is called when the cBot starts and is used for initialisation.
        protected override void OnStart()
        {
            // Initialise moving averages with specified source, periods and type.           
            fastMa = Indicators.MovingAverage(SourceSeries, FastPeriods, MAType);
            slowMa = Indicators.MovingAverage(SourceSeries, SlowPeriods, MAType);

            // Add the moving averages to the chart as indicators.            
            _indicator1 = Chart.Indicators.Add("Simple Moving Average", SourceSeries, FastPeriods, MAType);
            _indicator2 = Chart.Indicators.Add("Simple Moving Average", SourceSeries, SlowPeriods, MAType);

            // Customise the appearance of the fast moving average indicator line.
            _indicator1.Lines[0].Color = Color.Red;
            _indicator1.Lines[0].Thickness = 3;
        }

        // This method is called every time a bar closes.
        protected override void OnBarClosed()
        {
            // Remove the chart indicators each time a bar closes.            
            Chart.Indicators.Remove(_indicator1);
            Chart.Indicators.Remove(_indicator2);
        }

        // This method is called on every tick and is used for trade execution logic.
        protected override void OnTick()
        {
            // Find existing long and short positions for the symbol with the specified label.           
            var longPosition = Positions.Find(label, SymbolName, TradeType.Buy);
            var shortPosition = Positions.Find(label, SymbolName, TradeType.Sell);

            // Retrieve current and previous values for the slow and fast moving averages.
            var currentSlowMa = slowMa.Result.Last(0);
            var currentFastMa = fastMa.Result.Last(0);
            var previousSlowMa = slowMa.Result.Last(1);
            var previousFastMa = fastMa.Result.Last(1);

            // Trade logic based on moving average crossovers.

            // Buy condition - when the slow MA crosses below the fast MA and no long position exists.
            if (previousSlowMa > previousFastMa && currentSlowMa <= currentFastMa && longPosition == null)
            {
                if (shortPosition != null)
                    ClosePosition(shortPosition);  // Close any open short position.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, VolumeInUnits, label);  // Open a new market order to buy.
            }

            // Sell condition - when the slow MA crosses above the fast MA and no short position exists.
            else if (previousSlowMa < previousFastMa && currentSlowMa >= currentFastMa && shortPosition == null)
            {
                if (longPosition != null)
                    ClosePosition(longPosition);  // Close any open long position.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, VolumeInUnits, label);  // Open a new market order to sell.
            }
        }

        // Property to calculate and retrieve volume in units based on the specified volume in lots.      
        private double VolumeInUnits
        {
            // Convert the trade quantity in lots to volume in units.          
            get { return Symbol.QuantityToVolumeInUnits(Quantity); }
        }
    }
}
