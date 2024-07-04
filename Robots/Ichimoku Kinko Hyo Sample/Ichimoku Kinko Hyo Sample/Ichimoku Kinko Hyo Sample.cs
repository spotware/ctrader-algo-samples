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
    public class IchimokuKinkoHyoSample : Robot
    {
        private double _volumeInUnits;

        private IchimokuKinkoHyo _ichimokuKinkoHyo;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "IchimokuKinkoHyoSample")]
        public string Label { get; set; }

        [Parameter("Tenkan Sen Periods", DefaultValue = 9, Group = "IchimokuKinkoHyo", MinValue = 1)]
        public int TenkanSenPeriods { get; set; }

        [Parameter("Kijun Sen Periods", DefaultValue = 26, Group = "IchimokuKinkoHyo", MinValue = 1)]
        public int KijunSenPeriods { get; set; }

        [Parameter("Senkou Span B Periods", DefaultValue = 52, Group = "IchimokuKinkoHyo", MinValue = 1)]
        public int SenkouSpanBPeriods { get; set; }


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

            _ichimokuKinkoHyo = Indicators.IchimokuKinkoHyo(TenkanSenPeriods, KijunSenPeriods, SenkouSpanBPeriods);
        }

        protected override void OnBarClosed()
        {
            if (Bars.ClosePrices.Last(0) > _ichimokuKinkoHyo.SenkouSpanB.Last(0))
            {
                ClosePositions(TradeType.Sell);

                if (_ichimokuKinkoHyo.TenkanSen.Last(0) > _ichimokuKinkoHyo.KijunSen.Last(0) && _ichimokuKinkoHyo.TenkanSen.Last(1) <= _ichimokuKinkoHyo.KijunSen.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (Bars.ClosePrices.Last(0) < _ichimokuKinkoHyo.SenkouSpanA.Last(0))
            {
                ClosePositions(TradeType.Buy);

                if (_ichimokuKinkoHyo.TenkanSen.Last(0) < _ichimokuKinkoHyo.KijunSen.Last(0) && _ichimokuKinkoHyo.TenkanSen.Last(1) >= _ichimokuKinkoHyo.KijunSen.Last(1))
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