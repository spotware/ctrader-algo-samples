// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the Stochastic Oscillator to trade based on momentum signals. A buy position is
//    opened when %K crosses above %D in oversold territory, and a sell position is opened when %K 
//    crosses below %D in overbought territory.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class StochasticOscillatorSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private StochasticOscillator _stochasticOscillator;  // Store the Stochastic Oscillator indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "StochasticOscillatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("%K Periods", DefaultValue = 9, Group = "Stochastic Oscillator", MinValue = 1)]
        public int KPeriods { get; set; }  // Number of periods for %K, defaulting to 9.

        [Parameter("%K Slowing", DefaultValue = 3, Group = "Stochastic Oscillator", MinValue = 1)]
        public int KSlowing { get; set; }  // Smoothing factor for %K, defaulting to 3.

        [Parameter("%D Periods", DefaultValue = 9, Group = "Stochastic Oscillator", MinValue = 0)]
        public int DPeriods { get; set; }  // Number of periods for %D, defaulting to 9.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Stochastic Oscillator")]
        public MovingAverageType MAType { get; set; }  // Moving Average type used for smoothing %D.

        [Parameter("Down level", DefaultValue = 20, Group = "Stochastic Oscillator", MinValue = 1, MaxValue = 50)]
        public int DownValue { get; set; }  // Oversold level, defaulting to 20.

        [Parameter("Up level", DefaultValue = 80, Group = "Stochastic Oscillator", MinValue = 50, MaxValue = 100)]
        public int UpValue { get; set; }  // Overbought level, defaulting to 80.

        // This property finds all positions opened by this cBot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Finds positions with the same label used by the cBot.
            }
        }

        // This method is called when the cBot starts and is used for initialisation.
        protected override void OnStart()
        {
            // Convert the specified volume in lots to volume in units for the trading symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the Stochastic Oscillator with the specified parameters.
            _stochasticOscillator = Indicators.StochasticOscillator(KPeriods, KSlowing, DPeriods, MAType);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
             // Open a buy position if %K crosses above %D in oversold territory.
            if (_stochasticOscillator.PercentK.HasCrossedAbove(_stochasticOscillator.PercentD, 0) && _stochasticOscillator.PercentK.Last(1) <= DownValue)
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Open a sell position if %K crosses below %D in overbought territory.
            else if (_stochasticOscillator.PercentK.HasCrossedBelow(_stochasticOscillator.PercentD, 0) && _stochasticOscillator.PercentK.Last(1) >= UpValue)
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
