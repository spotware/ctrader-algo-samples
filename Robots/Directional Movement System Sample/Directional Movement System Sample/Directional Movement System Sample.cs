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
    public class DirectionalMovementSystemSample : Robot
    {
        private double _volumeInUnits;

        private DirectionalMovementSystem _directionalMovementSystem;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "DirectionalMovementSystemSample")]
        public string Label { get; set; }

        [Parameter(DefaultValue = 14, Group = "Directional Movement System", MinValue = 1, MaxValue = 10000)]
        public int Periods { get; set; }

        [Parameter("ADX Level", DefaultValue = 25, Group = "Directional Movement System")]
        public int ADXLevel { get; set; }

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

            _directionalMovementSystem = Indicators.DirectionalMovementSystem(Periods);
        }

        protected override void OnBarClosed()
        {
            if (_directionalMovementSystem.ADX.Last(0) < ADXLevel) return;

            if (_directionalMovementSystem.DIPlus.Last(0) > _directionalMovementSystem.DIMinus.Last(0) && _directionalMovementSystem.DIPlus.Last(1) <= _directionalMovementSystem.DIMinus.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_directionalMovementSystem.DIPlus.Last(0) < _directionalMovementSystem.DIMinus.Last(0) && _directionalMovementSystem.DIPlus.Last(1) >= _directionalMovementSystem.DIMinus.Last(1))
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