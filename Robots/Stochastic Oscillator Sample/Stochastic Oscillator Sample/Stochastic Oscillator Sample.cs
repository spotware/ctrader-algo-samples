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
    public class StochasticOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private StochasticOscillator _stochasticOscillator;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "StochasticOscillatorSample")]
        public string Label { get; set; }

        [Parameter("%K Periods", DefaultValue = 9, Group = "Stochastic Oscillator", MinValue = 1)]
        public int KPeriods { get; set; }

        [Parameter("%K Slowing", DefaultValue = 3, Group = "Stochastic Oscillator", MinValue = 1)]
        public int KSlowing { get; set; }

        [Parameter("%D Periods", DefaultValue = 9, Group = "Stochastic Oscillator", MinValue = 0)]
        public int DPeriods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Stochastic Oscillator")]
        public MovingAverageType MAType { get; set; }

        [Parameter("Down level", DefaultValue = 20, Group = "Stochastic Oscillator", MinValue = 1, MaxValue = 50)]
        public int DownValue { get; set; }

        [Parameter("Up level", DefaultValue = 80, Group = "Stochastic Oscillator", MinValue = 50, MaxValue = 100)]
        public int UpValue { get; set; }


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

            _stochasticOscillator = Indicators.StochasticOscillator(KPeriods, KSlowing, DPeriods, MAType);
        }

        protected override void OnBarClosed()
        {
            if (_stochasticOscillator.PercentK.HasCrossedAbove(_stochasticOscillator.PercentD, 0) && _stochasticOscillator.PercentK.Last(1) <= DownValue)
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_stochasticOscillator.PercentK.HasCrossedBelow(_stochasticOscillator.PercentD, 0) && _stochasticOscillator.PercentK.Last(1) >= UpValue)
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