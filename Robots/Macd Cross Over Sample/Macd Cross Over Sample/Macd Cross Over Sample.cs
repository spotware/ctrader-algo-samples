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
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class MacdCrossOverSample : Robot
    {
        private double _volumeInUnits;

        private MacdCrossOver _macdCrossOver;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "MacdCrossOverSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Macd Crossover")]
        public DataSeries Source { get; set; }

        [Parameter("Long Cycle", DefaultValue = 26, Group = "Macd Crossover", MinValue = 1)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", DefaultValue = 12, Group = "Macd Crossover", MinValue = 1)]
        public int ShortCycle { get; set; }

        [Parameter("Signal Periods", DefaultValue = 9, Group = "Macd Crossover", MinValue = 1)]
        public int SignalPeriods { get; set; }


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

            _macdCrossOver = Indicators.MacdCrossOver(Source, LongCycle, ShortCycle, SignalPeriods);
        }

        protected override void OnBarClosed()
        {
            if (_macdCrossOver.MACD.Last(0) > _macdCrossOver.Signal.Last(0) && _macdCrossOver.MACD.Last(1) <= _macdCrossOver.Signal.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_macdCrossOver.MACD.Last(0) < _macdCrossOver.Signal.Last(0) && _macdCrossOver.MACD.Last(1) >= _macdCrossOver.Signal.Last(1))
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
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