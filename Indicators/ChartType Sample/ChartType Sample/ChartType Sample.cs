// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
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
