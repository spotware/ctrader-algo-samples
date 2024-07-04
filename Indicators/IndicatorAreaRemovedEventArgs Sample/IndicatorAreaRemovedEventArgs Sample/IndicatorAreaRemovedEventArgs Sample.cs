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
