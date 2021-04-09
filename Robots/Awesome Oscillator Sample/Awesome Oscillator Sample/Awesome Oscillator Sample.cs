using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use an Awesome Oscillator indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class AwesomeOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private AwesomeOscillator _awesomeOscillator;

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

            _awesomeOscillator = Indicators.AwesomeOscillator();
        }

        protected override void OnBar()
        {
            foreach (var position in BotPositions)
            {
                if ((position.TradeType == TradeType.Buy && _awesomeOscillator.Result.Last(1) < _awesomeOscillator.Result.Last(2))
                    || (position.TradeType == TradeType.Sell && _awesomeOscillator.Result.Last(1) > _awesomeOscillator.Result.Last(2)))
                {
                    ClosePosition(position);
                }
            }

            if (_awesomeOscillator.Result.Last(1) > 0 && _awesomeOscillator.Result.Last(2) <= 0)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_awesomeOscillator.Result.Last(1) < 0 && _awesomeOscillator.Result.Last(2) >= 0)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
        }
    }
}