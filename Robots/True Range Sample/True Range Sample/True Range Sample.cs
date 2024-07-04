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
using System;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class TrueRangeSample : Robot
    {
        private double _volumeInUnits;

        private TrueRange _trueRange;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Label", DefaultValue = "TrueRangeSample")]
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

            _trueRange = Indicators.TrueRange();
        }

        protected override void OnBarClosed()
        {
            if (Bars.ClosePrices.Last(0) > Bars.OpenPrices.Last(0) && Bars.ClosePrices.Last(1) < Bars.OpenPrices.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteOrder(TradeType.Buy);
            }
            else if (Bars.ClosePrices.Last(0) < Bars.OpenPrices.Last(0) && Bars.ClosePrices.Last(1) > Bars.OpenPrices.Last(1))
            {
                ClosePositions(TradeType.Buy);

                ExecuteOrder(TradeType.Sell);
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

        private void ExecuteOrder(TradeType tradeType)
        {
            var trueRangeInPips = _trueRange.Result.Last(1) * (Symbol.TickSize / Symbol.PipSize * Math.Pow(10, Symbol.Digits));

            var stopLossInPips = trueRangeInPips * 2;
            var takeProfitInPips = stopLossInPips * 2;

            ExecuteMarketOrder(tradeType, SymbolName, _volumeInUnits, Label, stopLossInPips, takeProfitInPips);
        }
    }
}