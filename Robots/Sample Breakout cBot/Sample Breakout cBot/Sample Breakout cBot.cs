// -------------------------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you want to modify this file, please use the "Duplicate" functionality to make a copy. 
//
//    The Sample Breakout cBot checks the difference in pips between the top and bottom Bollinger Band and compares
//    it against the band height parameter specified by the user.  When the band height is lower than the number of  
//    pips specified, the market is considered to be consolidating, and the first candlestick to cross above the top   
//    band generates a buy signal, while crossing below the bottom band generates a sell signal.The user can specify   
//    the number of periods that the market should consolidate in the consolidation periods parameter. The position   
//    is closed by a stop loss or take profit.
//      
// -------------------------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class SampleBreakoutcBot : Robot
    {
        // Define the input parameters for the cBot.
        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }  // Trade quantity in lots.

        [Parameter("Stop Loss (pips)", Group = "Protection", DefaultValue = 20, MinValue = 1)]
        public int StopLossInPips { get; set; }  // Stop loss in pips.

        [Parameter("Take Profit (pips)", Group = "Protection", DefaultValue = 40, MinValue = 1)]
        public int TakeProfitInPips { get; set; }  // Take profit in pips.

        [Parameter("Source", Group = "Bollinger Bands")]
        public DataSeries Source { get; set; }  // Data series for Bollinger Bands.

        [Parameter("Band Height (pips)", Group = "Bollinger Bands", DefaultValue = 40.0, MinValue = 0)]
        public double BandHeightPips { get; set; }  // Height of Bollinger Bands in pips for a breakout signal.

        [Parameter("Bollinger Bands Deviations", Group = "Bollinger Bands", DefaultValue = 2)]
        public double Deviations { get; set; }  // Deviation value for Bollinger Bands.

        [Parameter("Bollinger Bands Periods", Group = "Bollinger Bands", DefaultValue = 20)]
        public int Periods { get; set; }  // Number of periods for Bollinger Bands.

        [Parameter("Bollinger Bands MA Type", Group = "Bollinger Bands")]
        public MovingAverageType MAType { get; set; }  // Moving average type for Bollinger Bands.

        [Parameter("Consolidation Periods", Group = "Bollinger Bands", DefaultValue = 2)]
        public int ConsolidationPeriods { get; set; }  // Number of consolidation periods required for a breakout signal.

        private BollingerBands _bollingerBands;  // Bollinger Bands indicator instance.
        private const string Label = "Sample Breakout cBot"; // Label to identify trade orders.
        private int _consolidation;  // Number of periods that have been in consolidation.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Initialise the Bollinger Bands indicator with the specified parameters.
            _bollingerBands = Indicators.BollingerBands(Source, Periods, Deviations, MAType);
        }

        // This method is called on each new bar.
        protected override void OnBar()
        {
            // Get the latest values of the top and bottom Bollinger Bands.
            var top = _bollingerBands.Top.Last(1);
            var bottom = _bollingerBands.Bottom.Last(1);

            // Check if the price is within the consolidation range (defined by BandHeightPips).
            if (top - bottom <= BandHeightPips * Symbol.PipSize)
            {
                // Increment for the consolidation counter if the price is within the band.
                _consolidation += 1;  
            }
            else
            {
                // Reset the consolidation counter if the price moves outside the band.
                _consolidation = 0;  
            }

            // If the consolidation lasts for the specified number of periods, check for breakout.
            if (_consolidation >= ConsolidationPeriods)
            {
                // Convert the trade quantity in lots to the volume in units.
                var volumeInUnits = Symbol.QuantityToVolumeInUnits(Quantity);

                // Breakout to the upside: if the ask price is above the top band, place a buy order.
                if (Ask > top)
                {
                    // Execute a buy market order with the specified trade type, symbol, volume, label, stop loss and take profit.
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                    _consolidation = 0;  // Reset the consolidation counter after placing a trade.
                }
                // Breakout to the downside: if the bid price is below the bottom band, place a sell order.
                else if (Bid < bottom)
                {
                    // Execute a sell market order with the specified trade type, symbol, volume, label, stop loss and take profit.
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                    _consolidation = 0;  // Reset the consolidation counter after placing a trade.
                }
            }
        }
    }
}
