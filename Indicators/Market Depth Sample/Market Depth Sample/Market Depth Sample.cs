// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MarketDepthSample : Indicator
    {
        private int _askNo;
        private int _bidNo;

        private MarketDepth _marketDepth;

        [Output("Bid Entries", LineColor = "Red", PlotType = PlotType.Histogram, Thickness = 5)]
        public IndicatorDataSeries BidResult { get; set; }

        [Output("Ask Entries", LineColor = "Blue", PlotType = PlotType.Histogram, Thickness = 5)]
        public IndicatorDataSeries AskResult { get; set; }

        protected override void Initialize()
        {
            _marketDepth = MarketData.GetMarketDepth(SymbolName);
            _marketDepth.Updated += MarketDepth_Updated;
            ;
        }

        private void MarketDepth_Updated()
        {
            _askNo = 0;
            _bidNo = 0;

            var index = Bars.ClosePrices.Count - 1;

            for (var i = 0; i < _marketDepth.AskEntries.Count; i++)
                AskResult[index - i] = double.NaN;

            foreach (var entry in _marketDepth.AskEntries)
            {
                AskResult[index - _askNo] = (-1) * entry.VolumeInUnits;
                _askNo++;
            }

            for (var i = 0; i < _marketDepth.BidEntries.Count; i++)
                BidResult[index - i] = double.NaN;

            foreach (var entry in _marketDepth.BidEntries)
            {
                BidResult[index - _bidNo] = entry.VolumeInUnits;
                _bidNo++;
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}
