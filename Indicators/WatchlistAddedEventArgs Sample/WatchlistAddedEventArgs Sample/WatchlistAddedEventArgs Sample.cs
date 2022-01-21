using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use WatchlistAddedEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WatchlistAddedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Watchlists.Added += Watchlists_Added;
        }

        private void Watchlists_Added(WatchlistAddedEventArgs obj)
        {
            Print("Watchlist {0} hase been added", obj.Watchlist.Name);
        }

        public override void Calculate(int index)
        {
        }
    }
}