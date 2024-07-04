// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WatchlistSample : Indicator
    {
        protected override void Initialize()
        {
            Print("Number of Watchlists: ", Watchlists.Count());

            foreach (var watchlist in Watchlists)
            {
                Print("Watchlist Name: {0} | Symbols #: {1}", watchlist.Name, watchlist.SymbolNames.Count);
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}
