// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The cBot uses the Volume Oscillator and Simple Moving Averages to identify potential trade
//    opportunities based on volume and price trends.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class VolumeOscillatorSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private VolumeOscillator _volumeOscillator;  // Store the Volume Oscillator indicator.

        private SimpleMovingAverage _priceSimpleMovingAverage;  // Store the SMA applied to price for trend detection.
        private SimpleMovingAverage _volumeOscillatorSimpleMovingAverage;  // Store the SMA applied to the Volume Oscillator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "VolumeOscillatorSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Short Term", DefaultValue = 9, Group = "Volume Oscillator", MinValue = 1)]
        public int ShortTerm { get; set; }  // Short-term period of the Volume Oscillator, defaulting to 9.

        [Parameter("Long Term", DefaultValue = 21, Group = "Volume Oscillator", MinValue = 1)]
        public int LongTerm { get; set; }  // Long-term period of the Volume Oscillator, defaulting to 21.

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

            // Initialise the Volume Oscillator with the specified periods.
            _volumeOscillator = Indicators.VolumeOscillator(ShortTerm, LongTerm);

            // Calculate a 14-period SMA of the Volume Oscillator.
            _volumeOscillatorSimpleMovingAverage = Indicators.SimpleMovingAverage(_volumeOscillator.Result, 14);

            // Calculate a 14-period SMA of the closing prices.
            _priceSimpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Skip trading if the Volume Oscillator is below its SMA.
            if (_volumeOscillator.Result.Last(0) < _volumeOscillatorSimpleMovingAverage.Result.Last(0)) return;

            // Identify a bullish signal: current close price crosses above the SMA.
            if (Bars.ClosePrices.Last(0) > _priceSimpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) <= _priceSimpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Identify a bearish signal: current close price crosses below the SMA.
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
