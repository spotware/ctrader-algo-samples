using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Momentum Oscillator indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MomentumOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private MomentumOscillator _momentumOscillator;

        private SimpleMovingAverage _simpleMovingAverage;

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

            _momentumOscillator = Indicators.MomentumOscillator(Bars.ClosePrices, 14);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(_momentumOscillator.Result, 14);
        }

        protected override void OnBar()
        {
            if (_momentumOscillator.Result.Last(1) > _simpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);

                if (_momentumOscillator.Result.Last(2) <= _simpleMovingAverage.Result.Last(2))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (_momentumOscillator.Result.Last(1) < _simpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Buy);

                if (_momentumOscillator.Result.Last(2) >= _simpleMovingAverage.Result.Last(2))
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