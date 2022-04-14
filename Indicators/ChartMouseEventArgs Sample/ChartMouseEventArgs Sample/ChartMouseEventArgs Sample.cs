using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartMouseEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartMouseEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.MouseMove += Chart_MouseMove;
            ;
        }

        private void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            var text = string.Format("Mouse Location: ({0}, {1})", obj.MouseX, obj.MouseY);

            Chart.DrawStaticText("mouse", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
