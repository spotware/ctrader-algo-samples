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
    public class VolumeIndexSample : Robot
    {
        private double _volumeInUnits;

        private PositiveVolumeIndex _positiveVolumeIndex;
        private NegativeVolumeIndex _negativeVolumeIndex;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "VolumeIndexSample")]
        public string Label { get; set; }

        [Parameter("Source for Positive Volume", Group = "Volume Index")]
        public DataSeries PositiveSource { get; set; }

        [Parameter("Source for Negative Volume", Group = "Volume Index")]
        public DataSeries NegativeSource { get; set; }

        [Parameter("Source", Group = "Simple Moving Average")]
        public DataSeries SourceSimpleMovingAverage { get; set; }

        [Parameter("Period", DefaultValue = 20, Group = "Simple Moving Average", MinValue = 1)]
        public int PeriodSimpleMovingAverage { get; set; }

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

            _positiveVolumeIndex = Indicators.PositiveVolumeIndex(PositiveSource);
            _negativeVolumeIndex = Indicators.NegativeVolumeIndex(NegativeSource);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(SourceSimpleMovingAverage, PeriodSimpleMovingAverage);
        }

        protected override void OnBarClosed()
        {
            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Sell);

                if (BotPositions.Length == 0 && _negativeVolumeIndex.Result.Last(0) > _positiveVolumeIndex.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);

                if (BotPositions.Length == 0 && _negativeVolumeIndex.Result.Last(0) < _positiveVolumeIndex.Result.Last(0))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
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