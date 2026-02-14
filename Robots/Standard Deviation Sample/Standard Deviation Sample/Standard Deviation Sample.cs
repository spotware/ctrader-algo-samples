// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates a trading strategy using Standard Deviation and Simple Moving Average (SMA).
//    It opens trades based on the crossing of closing prices above or below the SMA, with
//    risk management derived from the Standard Deviation.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone, AccessRights and its ability to add indicators.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class StandardDeviationSample : Robot
    {
        // Private fields for storing the indicator and trade volume.
        private double _volumeInUnits;  // Store volume in units calculated based on the specified lot size.

        private StandardDeviation _standardDeviation;  // Store the Standard Deviation indicator.

        private SimpleMovingAverage _simpleMovingAverage;  // Store the Simple Moving Average indicator.

        // Define input parameters for the cBot.
        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }  // Trade volume in lots, default value is 0.01.

        [Parameter("Label", DefaultValue = "StandardDeviationSample")]
        public string Label { get; set; }  // Unique label for identifying orders placed by this cBot.

        [Parameter("Source", Group = "Standard Deviation")]
        public DataSeries SourceStandardDeviation { get; set; }  // Data source for the Standard Deviation.

        [Parameter("Periods Standard Deviation", DefaultValue = 20, Group = "Standard Deviation", MinValue = 2)]
        public int PeriodsStandardDeviation { get; set; }  // Number of periods for the Standard Deviation, default is 20.

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Standard Deviation")]
        public MovingAverageType MATypeStandardDeviation { get; set; }  // Type of moving average, default is Simple.

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceMovingAverage { get; set; }  // Data source for the SMA.

        [Parameter("Periods Moving Average", DefaultValue = 14, Group = "Moving Average", MinValue = 2)]
        public int PeriodsMovingAverage { get; set; }  // Number of periods for the SMA, default is 14.

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

            // Initialise the Standard Deviation and SMA indicators with the specified parameters.
            _standardDeviation = Indicators.StandardDeviation(SourceStandardDeviation, PeriodsStandardDeviation, MATypeStandardDeviation);
            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceMovingAverage, PeriodsMovingAverage);
        }

        // This method is triggered whenever a bar is closed and drives the decision-making process for the cBot.
        protected override void OnBarClosed()
        {
            // Check if the closing price has crossed above the SMA on the most recent bar.
            if (Bars.ClosePrices.HasCrossedAbove(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);  // Close any open sell positions.

                ExecuteOrder(TradeType.Buy);  // Open a buy order.
            }
            
            // Check if the closing price has crossed below the SMA on the most recent bar.
            else if (Bars.ClosePrices.HasCrossedBelow(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Buy);  // Close any open buy positions.

                ExecuteOrder(TradeType.Sell);  // Open a sell order.
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

        // Execute a market order of the specified trade type with dynamically calculated stop loss 
        // and take profit levels based on the standard deviation of price movements.
        private void ExecuteOrder(TradeType tradeType)
        {
            // Convert the Standard Deviation value to pips for risk management.
            var standardDeviationInPips = _standardDeviation.Result.Last(1) * (Symbol.TickSize / Symbol.PipSize * Math.Pow(10, Symbol.Digits));

            // Set the stop loss to twice the Standard Deviation.
            var stopLossInPips = standardDeviationInPips * 2;

            // Set the take profit to twice the stop loss value.
            var takeProfitInPips = stopLossInPips * 2;

            // Open a market order with the calculated stop loss and take profit values.
            ExecuteMarketOrder(tradeType, SymbolName, _volumeInUnits, Label, stopLossInPips, takeProfitInPips);
        }
    }
}
