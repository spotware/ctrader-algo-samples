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
    public class PolynomialRegressionChannelsSample : Robot
    {
        private double _volumeInUnits;

        private PolynomialRegressionChannels _polynomialRegressionChannels;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Label", DefaultValue = "PolynomialRegressionChannelsSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Polynomial Regression Channels")]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 3.0, Group = "Polynomial Regression Channels", MinValue = 1, MaxValue = 4)]
        public int Degree { get; set; }

        [Parameter(DefaultValue = 120, Group = "Polynomial Regression Channels", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Standard Deviation", DefaultValue = 1.62, Group = "Polynomial Regression Channels", Step = 0.01)]
        public double StandardDeviation { get; set; }

        [Parameter("Standard Deviation 2", DefaultValue = 2, Group = "Polynomial Regression Channels", Step = 0.01)]
        public double StandardDeviation2 { get; set; }


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

            _polynomialRegressionChannels = Indicators.PolynomialRegressionChannels(Degree, Periods, StandardDeviation, StandardDeviation2);
        }

        protected override void OnBarClosed()
        {
            if (Bars.LowPrices.Last(0) <= _polynomialRegressionChannels.Sql.Last(0) && Bars.LowPrices.Last(1) > _polynomialRegressionChannels.Sql.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);
            }
            else if (Bars.HighPrices.Last(0) >= _polynomialRegressionChannels.Sqh.Last(0) && Bars.HighPrices.Last(1) < _polynomialRegressionChannels.Sqh.Last(1))
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