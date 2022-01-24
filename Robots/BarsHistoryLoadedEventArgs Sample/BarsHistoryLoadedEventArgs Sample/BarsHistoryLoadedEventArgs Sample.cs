using cAlgo.API;

namespace cAlgo.Robots
{
    // This sample indicator shows how to use BarsHistoryLoadedEventArgs
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class BarsHistoryLoadedEventArgsSample : Robot
    {
        protected override void OnStart()
        {
            Bars.HistoryLoaded += Bars_HistoryLoaded;

            // You can load more bars by calling this method or LoadMoreHistory
            Bars.LoadMoreHistoryAsync();
        }

        // This method will be called if you scroll left on your chart and load more data
        // Or if you call the Bars.LoadMoreHistory/LoadMoreHistoryAsync methods
        // BarsHistoryLoadedEventArgs has two properties
        // BarsHistoryLoadedEventArgs.Bars refers to the bars object that it's history is loaded
        // BarsHistoryLoadedEventArgs.Count gives you the number of bars that has been loaded
        private void Bars_HistoryLoaded(BarsHistoryLoadedEventArgs obj)
        {
            Print("Loaded Bars Count: {0}", obj.Count);
        }
    }
}