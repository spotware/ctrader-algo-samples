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
    public class UltimateOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private UltimateOscillator _ultimateOscillator;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "UltimateOscillatorSample")]
        public string Label { get; set; }

        [Parameter("Cycle 1", DefaultValue = 7, Group = "Ultimate Oscillator", MinValue = 1)]
        public int Cycle1 { get; set; }

        [Parameter("Cycle 2", DefaultValue = 14, Group = "Ultimate Oscillator", MinValue = 1)]
        public int Cycle2 { get; set; }

        [Parameter("Cycle 3", DefaultValue = 28, Group = "Ultimate Oscillator", MinValue = 1)]
        public int Cycle3 { get; set; }

        [Parameter("Down level", DefaultValue = 30, Group = "Stochastic Oscillator", MinValue = 1, MaxValue = 50)]
        public int DownValue { get; set; }

        [Parameter("Up level", DefaultValue = 70, Group = "Stochastic Oscillator", MinValue = 50, MaxValue = 100)]
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

            _ultimateOscillator = Indicators.UltimateOscillator(Cycle1, Cycle2, Cycle3);
        }

        protected override void OnBarClosed()
        {
            if (_ultimateOscillator.Result.Last(0) > UpValue && _ultimateOscillator.Result.Last(1) < UpValue)
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_ultimateOscillator.Result.Last(0) < DownValue && _ultimateOscillator.Result.Last(1) > DownValue)
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
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