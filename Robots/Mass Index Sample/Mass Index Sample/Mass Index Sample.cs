// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Mass Index indicator along with a Simple Moving Average (SMA) to detect
//    potential trend reversals. A sell trade is executed if the close price is above the SMA, and
//    a buy trade is executed if the close price is below the SMA.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class MassIndexSample : Robot
    {
        // Private fields for storing indicators and trade volume.
        private double _volumeInUnits;

        private MassIndex _massIndex;  // Store the Mass Index indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "MassIndexSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Periods", DefaultValue = 9, Group = "Mass Index", MinValue = 4)]
        public int Periods { get; set; }  // Number of periods for the Mass Index indicator, default is 9.

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceSimpleMovingAverage { get; set; }  // Data source for the SMA calculation.

        [Parameter("Periods", DefaultValue = 20, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }  // Number of periods for the SMA, default is 20.

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

            // Initialise the Mass Index indicator with the specified periods.
            _massIndex = Indicators.MassIndex(Periods);

            // Initialise the SMA with the specified data source and periods.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceSimpleMovingAverage, PeriodsSimpleMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Ensure the Mass Index has sufficient data before making decisions.
            if (_massIndex.Result.Last(0) < Periods) return;

            // If the close price is above the SMA, execute a sell trade.
            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
            }

            // If the close price is below the SMA, execute a buy trade.
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
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
