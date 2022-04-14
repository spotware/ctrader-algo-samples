using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartSizeEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartSizeEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.SizeChanged += Chart_SizeChanged;
        }

        private void Chart_SizeChanged(ChartSizeEventArgs obj)
        {
            Chart.DrawStaticText("size", "Size Changed", VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
