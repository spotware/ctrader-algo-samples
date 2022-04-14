using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartObjectsSelectionChangedEventArgs
    // Draw an object, select it and unselect it
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
