using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the High Minus Low indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class HighMinusLowSample : Robot
    {
        private double _volumeInUnits;

        private HighMinusLow _highMinusLow;

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

            _highMinusLow = Indicators.HighMinusLow();
        }

        protected override void OnBar()
        {
            if (_highMinusLow.Result.Last(1) < _highMinusLow.Result.Maximum(10)) return;

            if (Bars.ClosePrices.Last(1) > Bars.OpenPrices.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (Bars.ClosePrices.Last(1) < Bars.OpenPrices.Last(1))
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