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
    public class DetrendedPriceOscillatorSample : Robot
    {
        private double _volumeInUnits;

        private DetrendedPriceOscillator _detrendedPriceOscillator;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "DetrendedPriceOscillatorSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 21, Group = "Detrended Price Oscillator", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("MA Type", Group = "Detrended Price Oscillator")]
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

            _detrendedPriceOscillator = Indicators.DetrendedPriceOscillator(Bars.ClosePrices, Periods, MAType);
        }

        protected override void OnBarClosed()
        {
            if (_detrendedPriceOscillator.Result.Last(0) > 0 && _detrendedPriceOscillator.Result.Last(1) <= 0)
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_detrendedPriceOscillator.Result.Last(0) < 0 && _detrendedPriceOscillator.Result.Last(1) >= 0)
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