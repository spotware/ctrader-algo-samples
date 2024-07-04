// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
//    This example is based on the "Sample Trend cBot". The "Sample Trend cBot" will buy when fast period moving average crosses the slow period moving average and sell when
//    the fast period moving average crosses the slow period moving average. The orders are closed when an opposite signal
//    is generated. There can only by one Buy or Sell order at any time. The bot also includes a custom fitness function for optimisation.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;
using System.Linq;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CustomFitnessFunctions : Robot
    {
        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }

        [Parameter("MA Type", Group = "Moving Average")]
        public MovingAverageType MAType { get; set; }

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceSeries { get; set; }

        [Parameter("Slow Periods", Group = "Moving Average", DefaultValue = 10)]
        public int SlowPeriods { get; set; }

        [Parameter("Fast Periods", Group = "Moving Average", DefaultValue = 5)]
        public int FastPeriods { get; set; }

        private MovingAverage slowMa;
        private MovingAverage fastMa;
        private const string label = "Sample Trend cBot";

        protected override void OnStart()
        {
            fastMa = Indicators.MovingAverage(SourceSeries, FastPeriods, MAType);
            slowMa = Indicators.MovingAverage(SourceSeries, SlowPeriods, MAType);
        }

        protected override void OnTick()
        {
            var longPosition = Positions.Find(label, SymbolName, TradeType.Buy);
            var shortPosition = Positions.Find(label, SymbolName, TradeType.Sell);

            var currentSlowMa = slowMa.Result.Last(0);
            var currentFastMa = fastMa.Result.Last(0);
            var previousSlowMa = slowMa.Result.Last(1);
            var previousFastMa = fastMa.Result.Last(1);

            if (previousSlowMa > previousFastMa && currentSlowMa <= currentFastMa && longPosition == null)
            {
                if (shortPosition != null)
                    ClosePosition(shortPosition);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, VolumeInUnits, label);
            }
            else if (previousSlowMa < previousFastMa && currentSlowMa >= currentFastMa && shortPosition == null)
            {
                if (longPosition != null)
                    ClosePosition(longPosition);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, VolumeInUnits, label);
            }
        }

        private double VolumeInUnits
        {
            get { return Symbol.QuantityToVolumeInUnits(Quantity); }
        }

        protected override double GetFitness(GetFitnessArgs args)
        {
            if (args.TotalTrades > 20 && args.MaxEquityDrawdownPercentages < 50)
            {
                return Math.Pow(args.WinningTrades + 1, 2) / (args.LosingTrades + 1);
            }
            else
            {
                return double.MinValue;
            }
        }
    }
}