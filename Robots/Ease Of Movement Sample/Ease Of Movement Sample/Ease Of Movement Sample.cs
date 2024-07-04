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
    public class EaseOfMovementSample : Robot
    {
        private double _volumeInUnits;

        private EaseOfMovement _easeOfMovement;

        private MovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "EaseOfMovementSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 14, Group = "Ease Of Movement", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Ease Of Movement")]
        public MovingAverageType MAType { get; set; }


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

            _easeOfMovement = Indicators.EaseOfMovement(Periods, MAType);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 9);
        }

        protected override void OnBarClosed()
        {
            if (_easeOfMovement.Result.Last(0) > (Symbol.TickSize * 0.05))
            {
                if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) < _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
                else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) > _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else
            {
                ClosePositions();
            }
        }

        private void ClosePositions()
        {
            foreach (var position in BotPositions)
            {
                ClosePosition(position);
            }
        }
    }
}