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
    public class MoneyFlowIndexSample : Robot
    {
        private double _volumeInUnits;

        private MoneyFlowIndex _moneyFlowIndex;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "MoneyFlowIndexSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 14, Group = "Money Flow Index", MinValue = 2)]
        public int Periods { get; set; }

        [Parameter("Level Up", DefaultValue = 80, Group = "Money Flow Index", MinValue = 50, MaxValue = 100)]
        public int LevelUp { get; set; }

        [Parameter("Level Down", DefaultValue = 20, Group = "Money Flow Index", MinValue = 0, MaxValue = 50)]
        public int LevelDown { get; set; }


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

            _moneyFlowIndex = Indicators.MoneyFlowIndex(Periods);
        }

        protected override void OnBarClosed()
        {
            if (_moneyFlowIndex.Result.Last(0) > LevelUp)
            {
                ClosePositions(TradeType.Buy);

                if (BotPositions.Length == 0)
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (_moneyFlowIndex.Result.Last(0) < LevelDown)
            {
                ClosePositions(TradeType.Sell);

                if (BotPositions.Length == 0)
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
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