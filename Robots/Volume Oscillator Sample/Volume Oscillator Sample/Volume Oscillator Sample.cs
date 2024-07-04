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
    public class VolumeOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private VolumeOscillator _volumeOscillator;

        private SimpleMovingAverage _priceSimpleMovingAverage;
        private SimpleMovingAverage _volumeOscillatorSimpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "VolumeOscillatorSample")]
        public string Label { get; set; }

        [Parameter("Short Term", DefaultValue = 9, Group = "Volume Oscillator", MinValue = 1)]
        public int ShortTerm { get; set; }

        [Parameter("Long Term", DefaultValue = 21, Group = "Volume Oscillator", MinValue = 1)]
        public int LongTerm { get; set; }


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

            _volumeOscillator = Indicators.VolumeOscillator(ShortTerm, LongTerm);

            _volumeOscillatorSimpleMovingAverage = Indicators.SimpleMovingAverage(_volumeOscillator.Result, 14);

            _priceSimpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
        }

        protected override void OnBarClosed()
        {
            if (_volumeOscillator.Result.Last(0) < _volumeOscillatorSimpleMovingAverage.Result.Last(0)) return;

            if (Bars.ClosePrices.Last(0) > _priceSimpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) <= _priceSimpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (Bars.ClosePrices.Last(0) < _priceSimpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) >= _priceSimpleMovingAverage.Result.Last(1))
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