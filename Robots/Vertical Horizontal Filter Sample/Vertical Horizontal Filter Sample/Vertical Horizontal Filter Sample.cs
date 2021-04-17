using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Vertical Horizontal Filter indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class VerticalHorizontalFilterSample : Robot
    {
        private double _volumeInUnits;

        private VerticalHorizontalFilter _verticalHorizontalFilter;

        private SimpleMovingAverage _priceSimpleMovingAverage;
        private SimpleMovingAverage _verticalHorizontalFilterSimpleMovingAverage;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Label", DefaultValue = "Sample")]
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

            _verticalHorizontalFilter = Indicators.VerticalHorizontalFilter(Bars.ClosePrices, 28);

            _verticalHorizontalFilterSimpleMovingAverage = Indicators.SimpleMovingAverage(_verticalHorizontalFilter.Result, 14);

            _priceSimpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
        }

        protected override void OnBar()
        {
            if (_verticalHorizontalFilter.Result.Last(1) < _verticalHorizontalFilterSimpleMovingAverage.Result.Last(1)) return;

            if (Bars.ClosePrices.Last(1) > _priceSimpleMovingAverage.Result.Last(1) && Bars.ClosePrices.Last(2) <= _priceSimpleMovingAverage.Result.Last(2))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label, StopLossInPips, TakeProfitInPips);
            }
            else if (Bars.ClosePrices.Last(1) < _priceSimpleMovingAverage.Result.Last(1) && Bars.ClosePrices.Last(2) >= _priceSimpleMovingAverage.Result.Last(2))
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