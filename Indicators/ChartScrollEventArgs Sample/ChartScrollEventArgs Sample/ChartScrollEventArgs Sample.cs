using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartScrollEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartScrollEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ScrollChanged += Chart_ScrollChanged;
        }

        private void Chart_ScrollChanged(ChartScrollEventArgs obj)
        {
            Print("Scrolled, Bars Delta : {0} | Top Y Delta: {1} | Bottom Y Delta: {2}", obj.BarsDelta, obj.TopYDelta, obj.BottomYDelta);
        }

        public override void Calculate(int index)
        {
        }
    }
}