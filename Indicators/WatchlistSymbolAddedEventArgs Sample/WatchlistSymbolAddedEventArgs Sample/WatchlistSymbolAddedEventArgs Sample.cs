using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use WatchlistSymbolAddedEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WatchlistSymbolAddedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Watchlists.WatchlistSymbolAdded += Watchlists_WatchlistSymbolAdded;
        }

        private void Watchlists_WatchlistSymbolAdded(WatchlistSymbolAddedEventArgs obj)
        {
            Print("Symbol {0} Added to Watchlist {1}", obj.SymbolName, obj.Watchlist.Name);
        }

        public override void Calculate(int index)
        {
        }
    }
}
