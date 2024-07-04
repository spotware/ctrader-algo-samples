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
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class RelativeStrengthIndexSample : Robot
    {
        private double _volumeInUnits;

        private RelativeStrengthIndex _relativeStrengthIndex;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "RelativeStrengthIndexSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Relative Strength Index")]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 14, Group = "Relative Strength Index", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Down level", DefaultValue = 30, Group = "Relative Strength Index", MinValue = 1, MaxValue = 50)]
        public int DownValue { get; set; }

        [Parameter("Up level", DefaultValue = 70, Group = "Relative Strength Index", MinValue = 50, MaxValue = 100)]
        public int UpValue { get; set; }


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

            _relativeStrengthIndex = Indicators.RelativeStrengthIndex(Source, Periods);
        }

        protected override void OnBarClosed()
        {
            if (_relativeStrengthIndex.Result.Last(0) > UpValue && _relativeStrengthIndex.Result.Last(1) <= UpValue)
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_relativeStrengthIndex.Result.Last(0) < DownValue && _relativeStrengthIndex.Result.Last(1) >= DownValue)
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