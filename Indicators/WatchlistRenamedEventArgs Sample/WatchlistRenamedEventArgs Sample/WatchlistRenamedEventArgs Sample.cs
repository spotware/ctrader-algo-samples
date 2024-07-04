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
