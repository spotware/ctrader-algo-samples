using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This example shows how to use the Bars object BarOpened event BarOpenedEventArgs
    /// </summary>
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class BarOpenedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Bars.BarOpened += Bars_BarOpened;
        }

        // This method will be called if a new bar opens
        // BarOpenedEventArgs has a Bars property the you can use to get Bars object
        private void Bars_BarOpened(BarOpenedEventArgs obj)
        {
            var newOpendBar = obj.Bars.LastBar; // Or you can use obj.Bars[Bars.Count - 1] or obj.Bars.Last(0)
            var closedBar = obj.Bars.Last(1); // Or you can use obj.Bars[Bars.Count - 2]
        }

        public override void Calculate(int index)
        {
        }
    }
}