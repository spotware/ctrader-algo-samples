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
    public class CommodityChannelIndexSample : Robot
    {
        private double _volumeInUnits;

        private CommodityChannelIndex _commodityChannelIndex;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "CommodityChannelIndexSample")]
        public string Label { get; set; }

        [Parameter(DefaultValue = 20, Group = "Commodity Channel Index", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Down level", DefaultValue = -100, Group = "Commodity Channel Index")]
        public int DownValue { get; set; }

        [Parameter("Up level", DefaultValue = 100, Group = "Commodity Channel Index")]
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

            _commodityChannelIndex = Indicators.CommodityChannelIndex(Periods);
        }

        protected override void OnBarClosed()
        {
            if (_commodityChannelIndex.Result.Last(0) > UpValue && _commodityChannelIndex.Result.Last(1) <= UpValue)
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_commodityChannelIndex.Result.Last(0) < DownValue && _commodityChannelIndex.Result.Last(1) >= DownValue)
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