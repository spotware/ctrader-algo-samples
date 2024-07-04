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
