// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot uses two Welles Wilder Smoothing (WWS) indicators, one fast and one slow, to generate 
//    trade signals. A buy position is opened when the fast WWS crosses above the slow WWS and a 
//    sell position is opened when the fast WWS crosses below the slow WWS.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class WellesWilderSmoothingSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private WellesWilderSmoothing _fastWellesWilderSmoothing;  // Store the fast Welles Wilder Smoothing indicator.

        private WellesWilderSmoothing _slowWellesWilderSmoothing;  // Store the slow Welles Wilder Smoothing indicator.

        // Define input parameters for the cBot.
        [Parameter("Source", Group = "Fast MA")]
        public DataSeries FastMaSource { get; set; }  // Data source for the fast WWS.

        [Parameter("Period", DefaultValue = 9, Group = "Fast MA")]
        public int FastMaPeriod { get; set; }  // Period for the fast WWS, default is 9.

        [Parameter("Source", Group = "Slow MA")]
        public DataSeries SlowMaSource { get; set; }  // Data source for the slow WWS.

        [Parameter("Period", DefaultValue = 20, Group = "Slow MA")]
        public int SlowMaPeriod { get; set; }  // Period for the slow WWS, default is 20.

        [Parameter("Volume (Lots)", DefaultValue = 0.01, Group = "Trade")]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default is 0.01 lots.

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, Group = "Trade", MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }  // Stop-loss distance in pips, defaulting to 10 pips.

        [Parameter("Take Profit (Pips)", DefaultValue = 10, Group = "Trade", MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }  // Take-profit distance in pips, defaulting to 10 pips.

        [Parameter("Label", DefaultValue = "WellesWilderSmoothingSample", Group = "Trade")]
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

            // Initialise the fast and slow Welles Wilder Smoothing indicators with the specified periods.
            _fastWellesWilderSmoothing = Indicators.WellesWilderSmoothing(FastMaSource, FastMaPeriod);
            _slowWellesWilderSmoothing = Indicators.WellesWilderSmoothing(SlowMaSource, SlowMaPeriod);

            // Set the colors for the WWS indicators.
            _fastWellesWilderSmoothing.Result.Line.Color = Color.Blue;
            _slowWellesWilderSmoothing.Result.Line.Color = Color.Red;
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // If the fast WWS crosses above the slow WWS, execute a buy trade.
            if (_fastWellesWilderSmoothing.Result.HasCrossedAbove(_slowWellesWilderSmoothing.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);  // Open a market order to buy with the specified volume, stop loss and take profit.
            }
            
            // If the fast WWS crosses below the slow WWS, execute a sell trade.
            else if (_fastWellesWilderSmoothing.Result.HasCrossedBelow(_slowWellesWilderSmoothing.Result, 0))
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
