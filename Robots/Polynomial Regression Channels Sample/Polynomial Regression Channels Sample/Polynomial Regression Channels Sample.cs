using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Polynomial Regression Channels indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PolynomialRegressionChannelsSample : Robot
    {
        private double _volumeInUnits;

        private PolynomialRegressionChannels _polynomialRegressionChannels;

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

            _polynomialRegressionChannels = Indicators.PolynomialRegressionChannels(3, 120, 1.62, 2);
        }

        protected override void OnBar()
        {
            if (Bars.LowPrices.Last(1) <= _polynomialRegressionChannels.Sql.Last(1) && Bars.LowPrices.Last(2) > _polynomialRegressionChannels.Sql.Last(2))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);
            }
            else if (Bars.HighPrices.Last(1) >= _polynomialRegressionChannels.Sqh.Last(1) && Bars.HighPrices.Last(2) < _polynomialRegressionChannels.Sqh.Last(2))
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