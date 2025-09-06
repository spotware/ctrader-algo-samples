// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the MACD Crossover indicator to trade trend reversals. It opens a buy position when the MACD line
//    crosses above the Signal line and a sell position when the MACD line crosses below the Signal line.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class MacdCrossOverSample : Robot
    {
        // Private fields for storing indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private MacdCrossOver _macdCrossOver;  // Store the MACD Crossover indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, with a default of 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "MacdCrossOverSample")]
        public string Label { get; set; }  // Unique label for identifying orders opened by this bot.

        [Parameter("Source", Group = "Macd Crossover")]
        public DataSeries Source { get; set; }  // Source data series for the MACD calculation.

        [Parameter("Long Cycle", DefaultValue = 26, Group = "Macd Crossover", MinValue = 1)]
        public int LongCycle { get; set; }  // Long period for the MACD calculation, default is 26 periods.

        [Parameter("Short Cycle", DefaultValue = 12, Group = "Macd Crossover", MinValue = 1)]
        public int ShortCycle { get; set; }  // Short period for the MACD calculation, default is 12 periods.

        [Parameter("Signal Periods", DefaultValue = 9, Group = "Macd Crossover", MinValue = 1)]
        public int SignalPeriods { get; set; }  // Signal line smoothing period, default is 9 periods.

        // This property finds all positions opened by this bot, filtered by the Label parameter.
        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);  // Find positions with the same label used by the bot.
            }
        }

        // This method is called when the bot starts and is used for initialization.
        protected override void OnStart()
        {
            // Convert the volume in lots to the appropriate volume in units for the symbol.
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            // Initialise the MACD Crossover indicator with the specified parameters.
            _macdCrossOver = Indicators.MacdCrossOver(Source, LongCycle, ShortCycle, SignalPeriods);
        }

        // This method is triggered whenever a bar is closed and drives the bot’s decision-making.
        protected override void OnBarClosed()
        {
            // If the MACD line crosses above the Signal line, execute a buy trade.
            if (_macdCrossOver.MACD.Last(0) > _macdCrossOver.Signal.Last(0) && _macdCrossOver.MACD.Last(1) <= _macdCrossOver.Signal.Last(1))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a buy market order with the specified volume, stop loss and take profit.
            }

            // If the MACD line crosses below the Signal line, execute a sell trade.
            else if (_macdCrossOver.MACD.Last(0) < _macdCrossOver.Signal.Last(0) && _macdCrossOver.MACD.Last(1) >= _macdCrossOver.Signal.Last(1))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a sell market order with the specified volume, stop loss and take profit.
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
