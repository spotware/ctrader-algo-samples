using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the IndicatorAreaEventArgs
    // IndicatorAreaEventArgs is the base class of IndicatorAreaAddedEventArgs and IndicatorAreaRemovedEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class IndicatorAreaEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.IndicatorAreaAdded += Chart_IndicatorAreaAdded;
            Chart.IndicatorAreaRemoved += Chart_IndicatorAreaRemoved;
        }

        private void Chart_IndicatorAreaRemoved(IndicatorAreaRemovedEventArgs obj)
        {
            var indicatorAreaEventArgs = obj as IndicatorAreaEventArgs;
        }

        private void Chart_IndicatorAreaAdded(IndicatorAreaAddedEventArgs obj)
        {
            var indicatorAreaEventArgs = obj as IndicatorAreaEventArgs;
        }

        public override void Calculate(int index)
        {
        }
    }
}
