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
    public class MomentumOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private MomentumOscillator _momentumOscillator;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "MomentumOscillatorSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Momentum Oscillator")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", DefaultValue = 14, Group = "Momentum Oscillator", MinValue = 1)]
        public int PeriodsMomentumOscillator { get; set; }

        [Parameter("Periods", DefaultValue = 14, Group = "Simple Moving Average", MinValue = 0)]
        public int PeriodsSimpleMovingAverage { get; set; }


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

            _momentumOscillator = Indicators.MomentumOscillator(Source, PeriodsMomentumOscillator);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(_momentumOscillator.Result, PeriodsSimpleMovingAverage);
        }

        protected override void OnBarClosed()
        {
            if (_momentumOscillator.Result.Last(0) > _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Sell);

                if (_momentumOscillator.Result.Last(1) <= _simpleMovingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else if (_momentumOscillator.Result.Last(0) < _simpleMovingAverage.Result.Last(0))
            {
                ClosePositions(TradeType.Buy);

                if (_momentumOscillator.Result.Last(1) >= _simpleMovingAverage.Result.Last(1))
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