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
    public class PriceOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private PriceOscillator _priceOscillator;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "PriceOscillatorSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Price Oscillator")]
        public DataSeries Source { get; set; }

        [Parameter("Long Cycle", DefaultValue = 22, Group = "Price Oscillator", MinValue = 1)]
        public int LongCycle { get; set; }

        [Parameter("Short Cycle", DefaultValue = 14, Group = "Price Oscillator", MinValue = 1)]
        public int ShortCycle { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Price Oscillator")]
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

            _priceOscillator = Indicators.PriceOscillator(Source, LongCycle, ShortCycle, MAType);
        }

        protected override void OnBarClosed()
        {
            if (_priceOscillator.Result.Last(0) > 0 && _priceOscillator.Result.Last(1) <= 0)
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_priceOscillator.Result.Last(0) < 0 && _priceOscillator.Result.Last(1) >= 0)
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