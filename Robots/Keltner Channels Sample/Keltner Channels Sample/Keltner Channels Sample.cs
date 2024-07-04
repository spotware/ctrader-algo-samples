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
    public class KeltnerChannelsSample : Robot
    {
        private double _volumeInUnits;

        private KeltnerChannels _keltnerChannels;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Label", DefaultValue = "KeltnerChannelsSample")]
        public string Label { get; set; }

        [Parameter("MA Period", DefaultValue = 20, Group = "Keltner Channels", MinValue = 1)]
        public int MAPeriod { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple, Group = "Keltner Channels")]
        public MovingAverageType MAType { get; set; }

        [Parameter("ATR Period", DefaultValue = 10, Group = "Keltner Channels", MinValue = 1)]
        public int AtrPeriod { get; set; }

        [Parameter("ATR MA Type", DefaultValue = MovingAverageType.Simple, Group = "Keltner Channels")]
        public MovingAverageType AtrMAType { get; set; }

        [Parameter("Band Distance", DefaultValue = 2.0, MinValue = 0)]
        public double BandDistance { get; set; }



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

            _keltnerChannels = Indicators.KeltnerChannels(MAPeriod, MAType, AtrPeriod, AtrMAType, BandDistance);
        }

        protected override void OnBarClosed()
        {
            if (Bars.LowPrices.Last(0) <= _keltnerChannels.Bottom.Last(0) && Bars.LowPrices.Last(1) > _keltnerChannels.Bottom.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);
            }
            else if (Bars.HighPrices.Last(0) >= _keltnerChannels.Top.Last(0) && Bars.HighPrices.Last(1) < _keltnerChannels.Top.Last(1))
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