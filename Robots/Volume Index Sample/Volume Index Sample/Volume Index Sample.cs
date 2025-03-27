// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Positive and Negative Volume Index indicators, along with a Simple Moving 
//    Average, to execute trades based on volume and price trends.
//
// -------------------------------------------------------------------------------------------------


using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class VolumeIndexSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private PositiveVolumeIndex _positiveVolumeIndex;  // Store the Positive Volume Index (PVI) indicator.
        private NegativeVolumeIndex _negativeVolumeIndex;  // Store the Negative Volume Index (NVI) indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average (SMA) for trend detection.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "VolumeIndexSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source for Positive Volume", Group = "Volume Index")]
        public DataSeries PositiveSource { get; set; }  // Data source used by the Positive Volume Index.

        [Parameter("Source for Negative Volume", Group = "Volume Index")]
        public DataSeries NegativeSource { get; set; }  // Data source used by the Negative Volume Index.

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceSimpleMovingAverage { get; set; }  // Data source used by the SMA.

        [Parameter("Period", DefaultValue = 20, Group = "Simple Moving Average", MinValue = 1)]
        public int PeriodSimpleMovingAverage { get; set; }  // Number of periods for the SMA, defaulting to 20.

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

            // Initialise the Positive and Negative Volume Index indicators.
            _positiveVolumeIndex = Indicators.PositiveVolumeIndex(PositiveSource);
            _negativeVolumeIndex = Indicators.NegativeVolumeIndex(NegativeSource);

            // Initialise the Simple Moving Average indicator.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceSimpleMovingAverage, PeriodSimpleMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the latest close price is above the SMA.
            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Sell);  // Closу all sell positions.

                // Open a buy position if no active trades exist and NVI > PVI.
                if (BotPositions.Length == 0 && _negativeVolumeIndex.Result.Last(0) > _positiveVolumeIndex.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
                }
            }
            
            // Check if the latest close price is below the SMA.
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);  // Close all игн positions.

                // Open a sell position if no active trades exist and NVI < PVI.
                if (BotPositions.Length == 0 && _negativeVolumeIndex.Result.Last(0) < _positiveVolumeIndex.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to sell with the specified volume, stop loss and take profit.
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
