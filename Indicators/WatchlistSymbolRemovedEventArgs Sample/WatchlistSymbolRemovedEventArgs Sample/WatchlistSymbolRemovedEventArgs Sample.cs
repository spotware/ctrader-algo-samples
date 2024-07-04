// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WatchlistSymbolRemovedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Watchlists.WatchlistSymbolRemoved += Watchlists_WatchlistSymbolRemoved;
        }

        private void Watchlists_WatchlistSymbolRemoved(WatchlistSymbolRemovedEventArgs obj)
        {
            Print("Symbol {0} removed from watchlist {1}", obj.SymbolName, obj.Watchlist.Name);
        }

        public override void Calculate(int index)
        {
        }
    }
}
