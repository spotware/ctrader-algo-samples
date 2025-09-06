// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot implements a trading strategy that uses the Rainbow Oscillator indicator in combination
//    with a Simple Moving Average (SMA). The strategy executes trades based on the crossover of
//    the Rainbow Oscillator with the Simple Moving Average.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class RainbowOscillatorSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private RainbowOscillator _rainbowOscillator;  // Store the Rainbow Oscillator indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "RainbowOscillatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source", Group = "Rainbow Oscillator")]
        public DataSeries SourceRainbowOscillator { get; set; }  // Data source for the Rainbow Oscillator.

        [Parameter(MinValue = 2, DefaultValue = 9, Group = "Rainbow Oscillator")]
        public int LevelsRainbowOscillator { get; set; }  // Number of levels for the Rainbow Oscillator, defaulting to 9.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Rainbow Oscillator")]
        public MovingAverageType MATypeRainbowOscillator { get; set; }  // Type of moving average type, defaulting to Simple.

        [Parameter("Periods", DefaultValue = 9, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }  // Number of periods for calculating the SMA, default is 9.

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

            // Initialize the Rainbow Oscillator and Simple Moving Average indicators.
            _rainbowOscillator = Indicators.RainbowOscillator(SourceRainbowOscillator, LevelsRainbowOscillator, MATypeRainbowOscillator);
            _simpleMovingAverage = Indicators.SimpleMovingAverage(_rainbowOscillator.Result, PeriodsSimpleMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the Rainbow Oscillator has crossed above the SMA.
            if (_rainbowOscillator.Result.HasCrossedAbove(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to sell with the specified volume, stop loss and take profit.
            }
            
            // Check if the Rainbow Oscillator has crossed below the SMA.
            else if (_rainbowOscillator.Result.HasCrossedBelow(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
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
