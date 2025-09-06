// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Price Volume Trend (PVT) indicator along with a Simple Moving Average (SMA)
//    to detect potential trend reversals. It opens a buy position when the Price Volume Trend crosses 
//    above the SMA and a sell position when the Price Volume Trend crosses below the SMA.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class PriceVolumeTrendSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private PriceVolumeTrend _priceVolumeTrend;  // Store the Price Volume Trend indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "PriceVolumeTrendSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source", Group = "Price Volume Trend")]
        public DataSeries SourcePriceVolumeTrend { get; set; }  // Data series used as input for the PVT.

        [Parameter("Periods", DefaultValue = 20, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }  // Number of periods for calculating the SMA, default is 20.

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

            // Initialize the Price Volume Trend and Simple Moving Average indicators.
            _priceVolumeTrend = Indicators.PriceVolumeTrend(SourcePriceVolumeTrend);
            _simpleMovingAverage = Indicators.SimpleMovingAverage(_priceVolumeTrend.Result, PeriodsSimpleMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // If the Price Volume Trend crosses above the SMA, execute a buy trade.
            if (_priceVolumeTrend.Result.HasCrossedAbove(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // If the Price Volume Trend crosses below the SMA, execute a sell trade.
            else if (_priceVolumeTrend.Result.HasCrossedBelow(_simpleMovingAverage.Result, 0))
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
