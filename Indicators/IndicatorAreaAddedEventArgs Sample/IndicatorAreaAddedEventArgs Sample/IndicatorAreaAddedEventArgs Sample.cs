using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the IndicatorAreaAddedEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class IndicatorAreaAddedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            ShowIndicatorAreasCount();

            Chart.IndicatorAreaAdded += Chart_IndicatorAreaAdded;
        }

        private void Chart_IndicatorAreaAdded(IndicatorAreaAddedEventArgs obj)
        {
            Print("A new indicator area has been added");

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
