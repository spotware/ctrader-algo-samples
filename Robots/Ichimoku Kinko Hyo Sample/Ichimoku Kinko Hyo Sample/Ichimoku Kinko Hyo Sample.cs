using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Ichimoku Kinko Hyo indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class IchimokuKinkoHyoSample : Robot
    {
        private double _volumeInUnits;

        private IchimokuKinkoHyo _ichimokuKinkoHyo;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "Sample")]
        public string Label { get; set; }

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

            _ichimokuKinkoHyo = Indicators.IchimokuKinkoHyo(9, 26, 52);
        }

        protected override void OnBar()
        {
            if (Bars.ClosePrices.Last(1) > _ichimokuKinkoHyo.SenkouSpanB.Last(1))
            {
                ClosePositions(TradeType.Sell);

                if (_ichimokuKinkoHyo.TenkanSen.Last(1) > _ichimokuKinkoHyo.KijunSen.Last(1) && _ichimokuKinkoHyo.TenkanSen.Last(2) <= _ichimokuKinkoHyo.KijunSen.Last(2))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (Bars.ClosePrices.Last(1) < _ichimokuKinkoHyo.SenkouSpanA.Last(1))
            {
                ClosePositions(TradeType.Buy);

                if (_ichimokuKinkoHyo.TenkanSen.Last(1) < _ichimokuKinkoHyo.KijunSen.Last(1) && _ichimokuKinkoHyo.TenkanSen.Last(2) >= _ichimokuKinkoHyo.KijunSen.Last(2))
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