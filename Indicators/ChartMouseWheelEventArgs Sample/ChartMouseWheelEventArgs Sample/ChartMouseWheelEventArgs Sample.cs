using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartMouseWheelEventArgs
    // ChartMouseWheelEventArgs derives from ChartMouseEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartMouseWheelEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.MouseWheel += Chart_MouseWheel;
        }

        private void Chart_MouseWheel(ChartMouseWheelEventArgs obj)
        {
            var text = string.Format("Wheel Delta: {0}", obj.Delta);

            Chart.DrawStaticText("Wheel", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}