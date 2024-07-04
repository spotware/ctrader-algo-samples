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
    public class ParabolicSARSample : Robot
    {
        private double _volumeInUnits;

        private ParabolicSAR _parabolicSAR;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "ParabolicSARSample")]
        public string Label { get; set; }

        [Parameter("Min AF", DefaultValue = 0.02, Group = "Parabolic SAR", MinValue = 0)]
        public double MinAf { get; set; }

        [Parameter("Max AF", DefaultValue = 0.2, Group = "Parabolic SAR", MinValue = 0)]
        public double MaxAf { get; set; }


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

            _parabolicSAR = Indicators.ParabolicSAR(MinAf, MaxAf);
        }

        protected override void OnBarClosed()
        {
            if (_parabolicSAR.Result.Last(0) < Bars.LowPrices.Last(0) && _parabolicSAR.Result.Last(1) > Bars.HighPrices.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_parabolicSAR.Result.Last(0) > Bars.HighPrices.Last(0) && _parabolicSAR.Result.Last(1) < Bars.LowPrices.Last(1))
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