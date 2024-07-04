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

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class MassIndexSample : Robot
    {
        private double _volumeInUnits;

        private MassIndex _massIndex;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "MassIndexSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 9, Group = "Mass Index", MinValue = 4)]
        public int Periods { get; set; }

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceSimpleMovingAverage { get; set; }

        [Parameter("Periods", DefaultValue = 20, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }


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

            _massIndex = Indicators.MassIndex(Periods);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceSimpleMovingAverage, PeriodsSimpleMovingAverage);
        }

        protected override void OnBarClosed()
        {
            if (_massIndex.Result.Last(0) < Periods) return;

            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
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
    }
}