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
    public class AlligatorSample : Robot
    {
        private double _volumeInUnits;

        private Alligator _alligator;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "AlligatorSample")]
        public string Label { get; set; }

        [Parameter("Jaws Periods", DefaultValue = 13, Group = "Alligator", MinValue = 1)]
        public int JawsPeriods { get; set; }

        [Parameter("Jaws Shift", DefaultValue = 18, Group = "Alligator", MinValue = 0, MaxValue = 1000)]
        public int JawsShift { get; set; }

        [Parameter("Teeth Periods", DefaultValue = 8, Group = "Alligator", MinValue = 1)]
        public int TeethPeriods { get; set; }

        [Parameter("Teeth Shift", DefaultValue = 5, Group = "Alligator", MinValue = 0, MaxValue = 1000)]
        public int TeethShift { get; set; }

        [Parameter("Lips Periods", DefaultValue = 5, Group = "Alligator", MinValue = 1)]
        public int LipsPeriods { get; set; }

        [Parameter("Lips Shift", DefaultValue = 3, Group = "Alligator", MinValue = 0, MaxValue = 1000)]
        public int LipsShift { get; set; }


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

            _alligator = Indicators.Alligator(JawsPeriods, JawsShift, TeethPeriods, TeethShift, LipsPeriods, LipsShift);
        }

        protected override void OnBarClosed()
        {
            if (_alligator.Lips.Last(0) > _alligator.Teeth.Last(0) && _alligator.Lips.Last(1) <= _alligator.Teeth.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_alligator.Lips.Last(0) < _alligator.Teeth.Last(0) && _alligator.Lips.Last(1) >= _alligator.Teeth.Last(1))
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