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
    public class DonchianChannelSample : Robot
    {
        private double _volumeInUnits;

        private DonchianChannel _donchianChannel;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Label", DefaultValue = "DonchianChannelSample")]
        public string Label { get; set; }

        [Parameter("Periods", DefaultValue = 20, Group = "Donchian Channel", MinValue = 1)]
        public int Periods { get; set; }


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

            _donchianChannel = Indicators.DonchianChannel(Periods);
        }

        protected override void OnBarClosed()
        {
            if (Bars.LowPrices.Last(0) <= _donchianChannel.Bottom.Last(0) && Bars.LowPrices.Last(1) > _donchianChannel.Bottom.Last(1))
            {
                ClosePositions(TradeType.Sell);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, _volumeInUnits, Label);
            }
            else if (Bars.HighPrices.Last(0) >= _donchianChannel.Top.Last(0) && Bars.HighPrices.Last(1) < _donchianChannel.Top.Last(1))
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