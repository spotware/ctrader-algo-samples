using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartZoomEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartZoomEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ZoomChanged += Chart_ZoomChanged;
        }

        private void Chart_ZoomChanged(ChartZoomEventArgs obj)
        {
            var text = string.Format("Chart Zoom Level Changed To: {0}", obj.Chart.ZoomLevel);

            Chart.DrawStaticText("zoom", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
