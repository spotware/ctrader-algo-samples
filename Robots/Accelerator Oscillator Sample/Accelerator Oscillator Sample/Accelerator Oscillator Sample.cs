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
    public class AcceleratorOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private AcceleratorOscillator _acceleratorOscillator;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "AcceleratorOscillatorSample")]
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

            _acceleratorOscillator = Indicators.AcceleratorOscillator();
        }

        protected override void OnBarClosed()
        {
            foreach (var position in BotPositions)
            {
                if ((position.TradeType == TradeType.Buy && _acceleratorOscillator.Result.Last(0) < _acceleratorOscillator.Result.Last(1))
                    || (position.TradeType == TradeType.Sell && _acceleratorOscillator.Result.Last(0) > _acceleratorOscillator.Result.Last(1)))
                {
                    ClosePosition(position);
                }
            }

            if (_acceleratorOscillator.Result.Last(0) > 0 && _acceleratorOscillator.Result.Last(1) <= 0)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_acceleratorOscillator.Result.Last(0) < 0 && _acceleratorOscillator.Result.Last(1) >= 0)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
        }
    }
}