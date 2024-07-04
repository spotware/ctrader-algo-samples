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
    public class ChaikinVolatilitySample : Robot
    {
        private double _volumeInUnits;

        private ChaikinVolatility _chaikinVolatility;

        private MovingAverage _movingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "ChaikinVolatilitySample")]
        public string Label { get; set; }

        [Parameter(DefaultValue = 14, Group = "Chaikin Volatility", MinValue = 1)]
        public int ChaikinPeriods { get; set; }

        [Parameter("Rate of Change", DefaultValue = 10, Group = "Chaikin Volatility", MinValue = 0)]
        public int RateOfChange { get; set; }

        [Parameter("MA Type Chaikin", Group = "Chaikin Volatility")]
        public MovingAverageType MATypeChaikin { get; set; }

        [Parameter(DefaultValue = 14, Group = "Moving Average", MinValue = 1)]
        public int SmaPeriods { get; set; }

        [Parameter("MA Type", Group = "Moving Average")]
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

            _chaikinVolatility = Indicators.ChaikinVolatility(ChaikinPeriods, RateOfChange, MATypeChaikin);

            _movingAverage = Indicators.MovingAverage(Bars.ClosePrices, SmaPeriods, MAType);
        }

        protected override void OnBarClosed()
        {
            if (_chaikinVolatility.Result.Last(0) > 0)
            {
                if (Bars.ClosePrices.Last(0) > _movingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) < _movingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
                else if (Bars.ClosePrices.Last(0) < _movingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) > _movingAverage.Result.Last(1))
                {
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
                }
            }
            else
            {
                ClosePositions();
            }
        }

        private void ClosePositions()
        {
            foreach (var position in BotPositions)
            {
                ClosePosition(position);
            }
        }
    }
}