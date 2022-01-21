using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use WatchlistRenamedEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WatchlistRenamedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Watchlists.WatchlistRenamed += Watchlists_WatchlistRenamed;
        }

        private void Watchlists_WatchlistRenamed(WatchlistRenamedEventArgs obj)
        {
            Print("Watchlist renamed to {0}", obj.Watchlist.Name);
        }

        public override void Calculate(int index)
        {
        }
    }
}