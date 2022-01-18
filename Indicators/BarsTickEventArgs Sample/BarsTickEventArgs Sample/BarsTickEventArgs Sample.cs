using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Bars object Tick event BarsTickEventArgs
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class BarsTickEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            // For each tick the Bars_Tick will be called
            // Use this event if your indicator/cBot uses multiple time frames
            // or symbols bars, for current chart bars you can use indicators Calculate method
            // or cBots OnTick method
            Bars.Tick += Bars_Tick;
        }

        // This method will be called if a new tick arrives
        // BarsTickEventArgs has a Bars property the you can use to get Bars object
        // that it's tick changed, and a IsBarOpened property that allows you to
        // check if the current tick opened a new bar or not
        private void Bars_Tick(BarsTickEventArgs obj)
        {
            Print("Last Close Price: {0} | Is new Bar: {1}", obj.Bars.LastBar.Close, obj.IsBarOpened);
        }

        public override void Calculate(int index)
        {
        }
    }
}