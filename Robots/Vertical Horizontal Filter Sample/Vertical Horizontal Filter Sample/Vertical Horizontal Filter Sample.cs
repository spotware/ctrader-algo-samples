// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Vertical Horizontal Filter (VHF) indicator combined with a simple moving average
//    to identify trading conditions. It opens a buy position when the price crosses above the moving average 
//    and a sell position when it crosses below the moving average, provided the VHF is trending.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class VerticalHorizontalFilterSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private VerticalHorizontalFilter _verticalHorizontalFilter;  // Store the Vertical Horizontal Filter indicator.

        private SimpleMovingAverage _priceSimpleMovingAverage;  // Store the Simple Moving Average of closing prices.
        private SimpleMovingAverage _verticalHorizontalFilterSimpleMovingAverage;  // Store the Simple Moving Average of the VHF indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "VerticalHorizontalFilterSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source", Group = " Vertical Horizontal Filter")]
        public DataSeries Source { get; set; }  // Data source for the VHF indicator.

        [Parameter("Periods", DefaultValue = 28, Group = " Vertical Horizontal Filter", MinValue = 1)]
        public int Periods { get; set; }  // Number of periods, with a default of 28 periods.

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

            // Initialise the VHF indicator with the specified data source and period.
            _verticalHorizontalFilter = Indicators.VerticalHorizontalFilter(Source, Periods);

            // Initialise a simple moving average of the VHF result with a period of 14.
            _verticalHorizontalFilterSimpleMovingAverage = Indicators.SimpleMovingAverage(_verticalHorizontalFilter.Result, 14);

            // Initialise a simple moving average of closing prices with a period of 14.
            _priceSimpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Skip trading if the VHF is below its moving average, indicating a lack of trend.
            if (_verticalHorizontalFilter.Result.Last(0) < _verticalHorizontalFilterSimpleMovingAverage.Result.Last(1)) return;

            // Check if the closing price crosses above the moving average.
            if (Bars.ClosePrices.Last(0) > _priceSimpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) <= _priceSimpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Check if the closing price crosses below the moving average.
            else if (Bars.ClosePrices.Last(0) < _priceSimpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) >= _priceSimpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

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
