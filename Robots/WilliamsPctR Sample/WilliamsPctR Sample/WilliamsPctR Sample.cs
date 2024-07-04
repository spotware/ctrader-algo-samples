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
    public class WilliamsPctRSample : Robot
    {
        private double _volumeInUnits;

        private WilliamsPctR _williamsPctR;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "WilliamsPctRSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 14, Group = "Williams PctR", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Up level", DefaultValue = -20, Group = "Williams PctR")]
        public int UpValue { get; set; }

        [Parameter("Down level", DefaultValue = -80, Group = "Williams PctR")]
        public int DownValue { get; set; }

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

            _williamsPctR = Indicators.WilliamsPctR(Periods);
        }

        protected override void OnBarClosed()
        {
            if (_williamsPctR.Result.Last(0) > UpValue && _williamsPctR.Result.Last(1) < UpValue)
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_williamsPctR.Result.Last(0) < DownValue && _williamsPctR.Result.Last(1) > DownValue)
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