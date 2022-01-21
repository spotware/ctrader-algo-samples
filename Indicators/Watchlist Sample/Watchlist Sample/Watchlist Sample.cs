using cAlgo.API;
using System.Linq;

namespace cAlgo
{
    // This sample shows how to use a watchlist
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