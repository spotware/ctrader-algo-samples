using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SMATouchBot : Robot
    {
        // === INPUTS ===
        [Parameter("SMA Period", DefaultValue = 155)]
        public int smaPeriod { get; set; }

        [Parameter("TP (pips)", DefaultValue = 100)]
        public double tpPoints { get; set; }

        [Parameter("SL (pips)", DefaultValue = 50)]
        public double slPoints { get; set; }

        [Parameter("Trigger Step", DefaultValue = 10)]
        public double triggerStep { get; set; }

        [Parameter("SL Move", DefaultValue = 2)]
        public double slStep { get; set; }

        private SimpleMovingAverage sma;

        private double entryPrice;
        private double nextLevel;
        private double currentSL;

        private bool waitBuy = false;
        private bool waitSell = false;

        protected override void OnStart()
        {
            sma = Indicators.SimpleMovingAverage(Bars.ClosePrices, smaPeriod);
        }

        protected override void OnBar()
        {
            var last = Bars.Count - 1;

            double open = Bars.OpenPrices[last];
            double close = Bars.ClosePrices[last];
            double high = Bars.HighPrices[last];
            double low = Bars.LowPrices[last];

            double smaVal = sma.Result[last];

            bool inTrade = Positions.Count > 0;

            // === BUY SETUP ===
            if (low <= smaVal && open > smaVal && close > smaVal && !inTrade)
            {
                waitBuy = true;
                waitSell = false;
            }

            // === SELL SETUP ===
            if (high >= smaVal && open < smaVal && close < smaVal && !inTrade)
            {
                waitSell = true;
                waitBuy = false;
            }

            // === BUY ENTRY ===
            if (waitBuy && low <= smaVal && !inTrade)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000, "BUY", slPoints, tpPoints);

                entryPrice = close;
                currentSL = entryPrice - slPoints * Symbol.PipSize;
                nextLevel = entryPrice + triggerStep * Symbol.PipSize;

                waitBuy = false;
            }

            // === SELL ENTRY ===
            if (waitSell && high >= smaVal && !inTrade)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000, "SELL", slPoints, tpPoints);

                entryPrice = close;
                currentSL = entryPrice + slPoints * Symbol.PipSize;
                nextLevel = entryPrice - triggerStep * Symbol.PipSize;

                waitSell = false;
            }
        }

        protected override void OnTick()
        {
            if (Positions.Count == 0)
                return;

            var pos = Positions[0];

            double price = Symbol.Bid;

            // === BUY TRAILING ===
            if (pos.TradeType == TradeType.Buy)
            {
                if (price >= nextLevel)
                {
                    currentSL += slStep * Symbol.PipSize;
                    nextLevel += triggerStep * Symbol.PipSize;

                    ModifyPosition(pos, currentSL, pos.TakeProfit);
                }
            }

            // === SELL TRAILING ===
            if (pos.TradeType == TradeType.Sell)
            {
                if (price <= nextLevel)
                {
                    currentSL -= slStep * Symbol.PipSize;
                    nextLevel -= triggerStep * Symbol.PipSize;

                    ModifyPosition(pos, currentSL, pos.TakeProfit);
                }
            }
        }
    }
}
