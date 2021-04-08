using cAlgo.API;
using System.Linq;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Automate API to monitor user platform Watchlists
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WatchlistSample : Indicator
    {
        protected override void Initialize()
        {
            Watchlists.Added += Watchlists_Added;
            Watchlists.Removed += Watchlists_Removed;
            Watchlists.WatchlistRenamed += Watchlists_WatchlistRenamed;
            Watchlists.WatchlistSymbolAdded += Watchlists_WatchlistSymbolAdded;
            Watchlists.WatchlistSymbolRemoved += Watchlists_WatchlistSymbolRemoved;

            Print("Number of Watchlists: ", Watchlists.Count());

            foreach (var watchlist in Watchlists)
            {
                Print("Watchlist Name: {0} | Symbols #: {1}", watchlist.Name, watchlist.SymbolNames.Count);
            }
        }

        private void Watchlists_WatchlistSymbolRemoved(WatchlistSymbolRemovedEventArgs obj)
        {
            Print("Symbol {0} Removed From Watchlist {1}", obj.SymbolName, obj.Watchlist.Name);
        }

        private void Watchlists_WatchlistSymbolAdded(WatchlistSymbolAddedEventArgs obj)
        {
            Print("Symbol {0} Added to Watchlist {1}", obj.SymbolName, obj.Watchlist.Name);
        }

        private void Watchlists_WatchlistRenamed(WatchlistRenamedEventArgs obj)
        {
            Print("Renamed Watchlist {0}", obj.Watchlist.Name);
        }

        private void Watchlists_Removed(WatchlistRemovedEventArgs obj)
        {
            Print("Removed Watchlist {0}", obj.Watchlist.Name);
        }

        private void Watchlists_Added(WatchlistAddedEventArgs obj)
        {
            Print("Added Watchlist {0}", obj.Watchlist.Name);
        }

        public override void Calculate(int index)
        {
        }
    }
}