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
