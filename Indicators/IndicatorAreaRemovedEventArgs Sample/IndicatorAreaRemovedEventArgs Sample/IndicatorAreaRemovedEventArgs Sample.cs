using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the IndicatorAreaRemovedEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class IndicatorAreaRemovedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            ShowIndicatorAreasCount();

            Chart.IndicatorAreaRemoved += Chart_IndicatorAreaRemoved;
        }

        private void Chart_IndicatorAreaRemoved(IndicatorAreaRemovedEventArgs obj)
        {
            Print("An indicator area has been removed");

            ShowIndicatorAreasCount();
        }

        private void ShowIndicatorAreasCount()
        {
            var text = string.Format("Indicator Areas #: {0}", Chart.IndicatorAreas.Count);

            Chart.DrawStaticText("text", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}