using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartIconType
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartTypeSample : Indicator
    {
        protected override void Initialize()
        {
            ShowChartType();

            Chart.ChartTypeChanged += Chart_ChartTypeChanged;
        }

        private void Chart_ChartTypeChanged(ChartTypeEventArgs obj)
        {
            ShowChartType();
        }

        private void ShowChartType()
        {
            Chart.DrawStaticText("type", string.Format("Type: {0}", Chart.ChartType), VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
