// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Momentum Oscillator indicator along with a Simple Moving Average (SMA) to generate
//    trading signals. It executes buy orders when the Momentum Oscillator crosses above the SMA and sell
//    orders when the Momentum Oscillator crosses below the SMA.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class MomentumOscillatorSample : Robot
    {
        // Private fields for storing indicators and trade volume.
        private double _volumeInUnits;  // Store the volume in units based on the specified lot size.

        private MomentumOscillator _momentumOscillator;  // Store the Momentum Oscillator indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average indicator.

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, defaulting to 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "MomentumOscillatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this bot.

        [Parameter("Source", Group = "Momentum Oscillator")]
        public DataSeries Source { get; set; }  // Source for Momentum Oscillator, e.g., Close prices.

        [Parameter("Periods", DefaultValue = 14, Group = "Momentum Oscillator", MinValue = 1)]
        public int PeriodsMomentumOscillator { get; set; }  // Periods for the Momentum Oscillator, defaulting to 14.

        [Parameter("Periods", DefaultValue = 14, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }  // Periods for the Simple Moving Average, defaulting to 14.


        // This property retrieves all positions opened by this bot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label as this bot.
            }
        }

        // This method is called when the bot starts and is used for initialization.
        protected override void OnStart()
        {
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Momentum Oscillator indicator.
            _momentumOscillator = Indicators.MomentumOscillator(Source, PeriodsMomentumOscillator);

            // Initialise the Simple Moving Average indicator.
            _simpleMovingAverage = Indicators.SimpleMovingAverage(_momentumOscillator.Result, PeriodsSimpleMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // Check if the Momentum Oscillator is above the Simple Moving Average, signaling a buy opportunity.
            if (_momentumOscillator.Result.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                // Open a buy order only if the previous Momentum Oscillator value was below the SMA.
                if (_momentumOscillator.Result.Last(1) <= _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
                }
            }

            // Check if the Momentum Oscillator is below the Simple Moving Average, signaling a sell opportunity.
            else if (_momentumOscillator.Result.Last(0) < _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                // Open a sell order only if the previous Momentum Oscillator value was above the SMA.
                if (_momentumOscillator.Result.Last(1) >= _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
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
