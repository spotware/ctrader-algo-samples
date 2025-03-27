// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Chaikin Volatility indicator to detect significant volatility levels and
//    confirms trade entries using a moving average. It enters buy or sell trades based on price 
//    crossover with the moving average, combined with high volatility signals.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class ChaikinVolatilitySample : Robot
    {
        // Private fields to store trade volume and indicators.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private ChaikinVolatility _chaikinVolatility;  // Chaikin Volatility indicator to detect market volatility.

        private MovingAverage _movingAverage;  // Moving Average for trade entry confirmation.

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "ChaikinVolatilitySample")]
        public string Label { get; set; }  // Unique label for identifying orders opened by this bot.

        [Parameter(DefaultValue = 14, Group = "Chaikin Volatility", MinValue = 1)]
        public int ChaikinPeriods { get; set; }  // Periods for Chaikin Volatility calculation, defaulting to 14.

        [Parameter("Rate of Change", DefaultValue = 10, Group = "Chaikin Volatility", MinValue = 0)]
        public int RateOfChange { get; set; }  // Rate of change for Chaikin Volatility, defaulting to 10.

        [Parameter("MA Type Chaikin", Group = "Chaikin Volatility")]
        public MovingAverageType MATypeChaikin { get; set; }  // Type of moving average for Chaikin Volatility.

        [Parameter(DefaultValue = 14, Group = "Moving Average", MinValue = 1)]
        public int SmaPeriods { get; set; }  // Periods for the moving average used in trade confirmation, defaulting to 14.

        [Parameter("MA Type", Group = "Moving Average")]
        public MovingAverageType MAType { get; set; }  // Type of moving average for trade confirmation.

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

            // Initialise the Chaikin Volatility indicator with the specified parameters.
            _chaikinVolatility = Indicators.ChaikinVolatility(ChaikinPeriods, RateOfChange, MATypeChaikin);

            // Initialise the Moving Average indicator with the specified parameters.
            _movingAverage = Indicators.MovingAverage(Bars.ClosePrices, SmaPeriods, MAType);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Execute trades only when volatility is high (Chaikin Volatility > 0).
            if (_chaikinVolatility.Result.Last(0) > 0)
            {
                // Buy signal: price crosses above moving average.
                if (Bars.ClosePrices.Last(0) > _movingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) < _movingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
                }

                // Sell signal: price crosses below moving average.
                else if (Bars.ClosePrices.Last(0) < _movingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) > _movingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
                }
            }

            // Close all positions when volatility condition is not met.
            else
            {
                ClosePositions();  // Close all positions.
            }
        }

        // This method closes all open positions that were created by this bot.
        private void ClosePositions()
        {
            foreach (var position in BotPositions)
            {
                ClosePosition(position);  // Close the position.
            }
        }
    }
}
