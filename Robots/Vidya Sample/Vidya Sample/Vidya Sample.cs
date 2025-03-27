// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses the VIDYA (Variable Index Dynamic Average) indicator to determine trend direction.
//    It opens buy and sell positions based on the crossover of two VIDYA lines with different settings.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class VidyaSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private Vidya _fastVidya;  // Store the fast VIDYA indicator.

        private Vidya _slowVidya;  // Store the slow VIDYA indicator.

        // Define input parameters for the cBot.
        [Parameter("Source", Group = "Fast MA")]
        public DataSeries FastMaSource { get; set; }  // Data source for the fast VIDYA indicator.

        [Parameter("Period", DefaultValue = 9, Group = "Fast MA")]
        public int FastMaPeriod { get; set; }  // Period for the fast VIDYA, defaulting to 9.

        [Parameter("Source", Group = "Slow MA")]
        public DataSeries SlowMaSource { get; set; }  // Data source for the slow VIDYA indicator.

        [Parameter("Period", DefaultValue = 20, Group = "Slow MA")]
        public int SlowMaPeriod { get; set; }  // Period for the slow VIDYA, defaulting to 20.

        [Parameter("Sigma", DefaultValue = 0.65, Group = "Fast MA", MinValue = 0.1, MaxValue = 0.95)]
        public double FastSigma { get; set; }  // Sensitivity (Sigma) of the fast VIDYA, defaulting to 0.65.

        [Parameter("Sigma", DefaultValue = 0.6, Group = "Slow MA", MinValue = 0.1, MaxValue = 0.95)]
        public double SlowSigma { get; set; }  // Sensitivity (Sigma) of the slow VIDYA, defaulting to 0.65.

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "VidyaSample", Group = "Trade")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

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

            // Initialise the fast and slow VIDYA indicator with the specified parameters.
            _fastVidya = Indicators.Vidya(FastMaSource, FastMaPeriod, FastSigma);
            _slowVidya = Indicators.Vidya(SlowMaSource, SlowMaPeriod, SlowSigma);

            // Set the display colors for the fast and slow VIDYA lines.
            _fastVidya.Result.Line.Color = Color.Blue;
            _slowVidya.Result.Line.Color = Color.Red;
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the fast VIDYA crosses above the slow VIDYA.
            if (_fastVidya.Result.HasCrossedAbove(_slowVidya.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close all sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // Check if the fast VIDYA crosses below the slow VIDYA.
            else if (_fastVidya.Result.HasCrossedBelow(_slowVidya.Result, 0))
            {
                ClosePositions(TradeType.Buy);  // Close all buy positions.

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
