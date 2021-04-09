using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use an Bollinger Bands indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class BollingerBandsSample : Robot
    {
        private double _volumeInUnits;

        private BollingerBands _bollingerBands;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Label", DefaultValue = "Sample")]
        public string Label { get; set; }

        [Parameter("Source")]
        public DataSeries Source { get; set; }

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

            _bollingerBands = Indicators.BollingerBands(Source, 14, 2, MovingAverageType.Exponential);
        }

        protected override void OnBar()
        {
            if (Bars.LowPrices.Last(1) <= _bollingerBands.Bottom.Last(1) && Bars.LowPrices.Last(2) > _bollingerBands.Bottom.Last(2))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);
            }
            else if (Bars.HighPrices.Last(1) >= _bollingerBands.Top.Last(1) && Bars.HighPrices.Last(2) < _bollingerBands.Top.Last(2))
            {
                ClosePositions(TradeType.Buy);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, _volumeInUnits, Label);
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