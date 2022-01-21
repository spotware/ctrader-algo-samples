using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use WatchlistRemovedEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WatchlistRemovedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Watchlists.Removed += Watchlists_Removed;
        }

        private void Watchlists_Removed(WatchlistRemovedEventArgs obj)
        {
            Print("Watchlist {0} removed", obj.Watchlist.Name);
        }

        public override void Calculate(int index)
        {
        }
    }
}