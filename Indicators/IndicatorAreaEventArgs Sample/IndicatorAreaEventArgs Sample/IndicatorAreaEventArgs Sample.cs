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
