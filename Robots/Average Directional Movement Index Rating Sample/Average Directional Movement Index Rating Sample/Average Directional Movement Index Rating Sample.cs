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
    public class AverageDirectionalMovementIndexRatingSample : Robot
    {
        private double _volumeInUnits;

        private AverageDirectionalMovementIndexRating _averageDirectionalMovementIndexRating;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "AverageDirectionalMovementIndexRatingSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 14, Group = "Average Directional Movement Index Ratin")]
        public int Periods { get; set; }

        [Parameter("ADXR Level", DefaultValue = 25, Group = "Average Directional Movement Index Ratin")]
        public int ADXRLevel { get; set; }


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

            _averageDirectionalMovementIndexRating = Indicators.AverageDirectionalMovementIndexRating(20);
        }

        protected override void OnBarClosed()
        {
            if (_averageDirectionalMovementIndexRating.ADXR.Last(0) < ADXRLevel) return;

            if (_averageDirectionalMovementIndexRating.DIPlus.Last(0) > _averageDirectionalMovementIndexRating.DIMinus.Last(0) && _averageDirectionalMovementIndexRating.DIPlus.Last(1) <= _averageDirectionalMovementIndexRating.DIMinus.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (_averageDirectionalMovementIndexRating.DIPlus.Last(0) < _averageDirectionalMovementIndexRating.DIMinus.Last(0) && _averageDirectionalMovementIndexRating.DIPlus.Last(1) >= _averageDirectionalMovementIndexRating.DIMinus.Last(1))
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