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
    public class HistoricalVolatilitySample : Robot
    {
        private double _volumeInUnits;

        private HistoricalVolatility _historicalVolatility;

        private SimpleMovingAverage _simpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10, MaxValue = 100, MinValue = 1, Step = 1)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "HistoricalVolatilitySample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 20, Group = "Historical Volatility", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Bar History", DefaultValue = 252, Group = "Historical Volatility")]
        public int BarHistory { get; set; }


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

            _historicalVolatility = Indicators.HistoricalVolatility(Bars.ClosePrices, Periods, BarHistory);

            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 9);
        }

        protected override void OnBarClosed()
        {
            if (_historicalVolatility.Result.Last(0) < _historicalVolatility.Result.Maximum(14)) return;

            if (Bars.ClosePrices.Last(0) > _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) < _simpleMovingAverage.Result.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (Bars.ClosePrices.Last(0) < _simpleMovingAverage.Result.Last(0) && Bars.ClosePrices.Last(1) > _simpleMovingAverage.Result.Last(1))
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