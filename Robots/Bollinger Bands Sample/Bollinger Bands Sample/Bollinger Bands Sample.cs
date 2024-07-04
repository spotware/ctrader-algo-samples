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
    public class BollingerBandsSample : Robot
    {
        private double _volumeInUnits;

        private BollingerBands _bollingerBands;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Label", DefaultValue = "BollingerBandsSample")]
        public string Label { get; set; }

        [Parameter("Source", Group = "Bollinger Bands")]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 20, Group = "Bollinger Bands", MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Standard Dev", DefaultValue = 2.0, Group = "Bollinger Bands", MinValue = 0.0001, MaxValue = 10)]
        public double StandardDeviations { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Bollinger Bands")]
        public MovingAverageType MAType { get; set; }

        [Parameter("Shift", DefaultValue = 0, Group = "Bollinger Bands", MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }


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

            _bollingerBands = Indicators.BollingerBands(Source, Periods, StandardDeviations, MAType);
        }

        protected override void OnBarClosed()
        {
            if (Bars.LowPrices.Last(0) <= _bollingerBands.Bottom.Last(0) && Bars.LowPrices.Last(1) > _bollingerBands.Bottom.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);
            }
            else if (Bars.HighPrices.Last(0) >= _bollingerBands.Top.Last(0) && Bars.HighPrices.Last(1) < _bollingerBands.Top.Last(1))
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