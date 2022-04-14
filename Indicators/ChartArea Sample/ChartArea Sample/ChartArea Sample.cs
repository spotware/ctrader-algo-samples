using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the ChartArea
    // ChartArea is the base class of Chart and IndicatorArea
    // Both Chart and IndicatorArea inherits most of their members from ChartArea
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartAreaSample : Indicator
    {
        protected override void Initialize()
        {
            DrawStaticText(Chart, "Chart");
            DrawStaticText(IndicatorArea, "Indicator");
        }

        private void DrawStaticText(ChartArea chartArea, string text)
        {
            chartArea.DrawStaticText(text, text, VerticalAlignment.Center, HorizontalAlignment.Center, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
