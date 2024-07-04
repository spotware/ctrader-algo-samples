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
