using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Macd Cross Over indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MacdCrossOverSample : Robot
    {
        private double _volumeInUnits;

        private MacdCrossOver _macdCrossOver;

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

            _macdCrossOver = Indicators.MacdCrossOver(Bars.ClosePrices, 26, 12, 9);
        }

        protected override void OnBar()
        {
            if (_macdCrossOver.MACD.Last(1) > _macdCrossOver.Signal.Last(1) && _macdCrossOver.MACD.Last(2) <= _macdCrossOver.Signal.Last(2))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_macdCrossOver.MACD.Last(1) < _macdCrossOver.Signal.Last(1) && _macdCrossOver.MACD.Last(2) >= _macdCrossOver.Signal.Last(2))
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