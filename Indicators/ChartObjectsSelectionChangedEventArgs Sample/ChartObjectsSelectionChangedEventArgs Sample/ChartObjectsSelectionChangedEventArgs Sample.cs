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
    public class ChartObjectsSelectionChangedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ObjectsSelectionChanged += Chart_ObjectsSelectionChanged;
        }

        private void Chart_ObjectsSelectionChanged(ChartObjectsSelectionChangedEventArgs obj)
        {
            Print("Added objects #: {0} | Removed Objects #: {1}", obj.ObjectsAddedToSelection.Count, obj.ObjectsRemovedFromSelection.Count);
        }

        public override void Calculate(int index)
        {
        }
    }
}
