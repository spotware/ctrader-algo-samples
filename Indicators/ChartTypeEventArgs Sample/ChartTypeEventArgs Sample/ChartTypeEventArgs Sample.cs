using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartTypeEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartTypeEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ChartTypeChanged += Chart_ChartTypeChanged;
        }

        private void Chart_ChartTypeChanged(ChartTypeEventArgs obj)
        {
            var text = string.Format("Chart Type Changed To: {0}", obj.Chart.ChartType);

            Chart.DrawStaticText("type", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
