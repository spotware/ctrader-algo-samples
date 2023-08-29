using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    // This sample indicator shows how to get a symbol and time frame market data
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MarketDataSample : Indicator
    {
        private Bars _bars;
        private Ticks _ticks;
        private MarketDepth _marketDepth;

        protected override void Initialize()
        {
            _bars = MarketData.GetBars(TimeFrame, SymbolName);
            _ticks = MarketData.GetTicks(SymbolName);
            _marketDepth = MarketData.GetMarketDepth(SymbolName);

            Print(_bars.ClosePrices.Last());
            Print(_bars.Last());
            
            var infoBars = _bars.ToList();
            foreach (var bar in infoBars)
            {
                Print($"Open: {bar.Open}, High: {bar.High}, Low: {bar.Low}, Close: {bar.Close}");
            }
            
            
            var infoTicks = _ticks.ToList();
            foreach (var tick in infoTicks)
            {
                Print($"Time {tick.Time}, Ask {tick.Ask}, Bid {tick.Bid}");
            }
            
            var infoMarketDepth = _marketDepth.AskEntries.ToList();
            foreach (var marketDepth in infoMarketDepth)
            {
                Print($"Price {marketDepth.Price}, Volume {marketDepth.VolumeInUnits}");
            }

        }

        public override void Calculate(int index)
        {
        }
    }
}
