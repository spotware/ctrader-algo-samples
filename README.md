using cAlgo.API;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FirstTickBuyBot : Robot
    {
        private bool _tradeOpened = false;

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Take Profit %", DefaultValue = 0.10)]
        public double TakeProfitPercent { get; set; }

        [Parameter("Stop Loss %", DefaultValue = 0.05)]
        public double StopLossPercent { get; set; }

        protected override void OnTick()
        {
            // Only run once (on the first tick)
            if (_tradeOpened)
                return;

            double entryPrice = Symbol.Bid;
            long volumeInUnits = Symbol.QuantityToVolume(VolumeInLots);

            // Calculate TP and SL in price terms
            double tpPrice = entryPrice + (entryPrice * TakeProfitPercent / 100.0);
            double slPrice = entryPrice - (entryPrice * StopLossPercent / 100.0);

            // Place Buy Order with TP and SL
            ExecuteMarketOrder(TradeType.Buy, SymbolName, volumeInUnits, "FirstTickBuy", slPrice, tpPrice);

            _tradeOpened = true;
        }
    }
}
