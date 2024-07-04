// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class StandardDeviationSample : Robot
    {
        private double _volumeInUnits;

        private StandardDeviation _standardDeviation;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Label", DefaultValue = "StandardDeviationSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Standard Deviation")]
        public DataSeries SourceStandardDeviation { get; set; }

        [Parameter("Periods Standard Deviation", DefaultValue = 20, Group = "Standard Deviation", MinValue = 2)]
        public int PeriodsStandardDeviation { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Standard Deviation")]
        public MovingAverageType MATypeStandardDeviation { get; set; }

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceMovingAverage { get; set; }

        [Parameter("Periods Moving Average", DefaultValue = 14, Group = "Moving Average", MinValue = 2)]
        public int PeriodsMovingAverage { get; set; }


        public Position[] BotPositions
        {
            get
            {
                return Positions.FindAll(Label);
            }
        }

        protected override void OnStart()
        {
            _volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            _standardDeviation = Indicators.StandardDeviation(SourceStandardDeviation, PeriodsStandardDeviation, MATypeStandardDeviation);
            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceMovingAverage, PeriodsMovingAverage);
        }

        protected override void OnBarClosed()
        {
            if (Bars.ClosePrices.HasCrossedAbove(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Sell);

                ExecuteOrder(TradeType.Buy);
            }
            else if (Bars.ClosePrices.HasCrossedBelow(_simpleMovingAverage.Result, 0))
            {
                ClosePositions(TradeType.Buy);

                ExecuteOrder(TradeType.Sell);
            }
        }

        private void ClosePositions(TradeType tradeType)
        {
            foreach (var position in BotPositions)
            {
                if (position.TradeType != tradeType) continue;

                ClosePosition(position);
            }
        }

        private void ExecuteOrder(TradeType tradeType)
        {
            var standardDeviationInPips = _standardDeviation.Result.Last(1) * (Symbol.TickSize / Symbol.PipSize * Math.Pow(10, Symbol.Digits));

            var stopLossInPips = standardDeviationInPips * 2;
            var takeProfitInPips = stopLossInPips * 2;

            ExecuteMarketOrder(tradeType, SymbolName, _volumeInUnits, Label, stopLossInPips, takeProfitInPips);
        }
    }
}