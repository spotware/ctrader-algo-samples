using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the Keltner Channels indicator
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class KeltnerChannelsSample : Robot
    {
        private double _volumeInUnits;

        private KeltnerChannels _keltnerChannels;

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

            _keltnerChannels = Indicators.KeltnerChannels(20, MovingAverageType.Exponential, 10, MovingAverageType.Simple, 2);
        }

        protected override void OnBar()
        {
            if (Bars.LowPrices.Last(1) <= _keltnerChannels.Bottom.Last(1) && Bars.LowPrices.Last(2) > _keltnerChannels.Bottom.Last(2))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);
            }
            else if (Bars.HighPrices.Last(1) >= _keltnerChannels.Top.Last(1) && Bars.HighPrices.Last(2) < _keltnerChannels.Top.Last(2))
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