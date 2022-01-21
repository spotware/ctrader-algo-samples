using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use WatchlistSymbolRemovedEventArgs
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