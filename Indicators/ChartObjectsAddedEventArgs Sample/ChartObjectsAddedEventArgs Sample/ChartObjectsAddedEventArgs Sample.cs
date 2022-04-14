using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartObjectsAddedEventArgs
    // ChartObjectsAddedEventArgs is derived from ChartObjectsEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartObjectsAddedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ObjectsAdded += Chart_ObjectsAdded;
            ;
        }

        private void Chart_ObjectsAdded(ChartObjectsAddedEventArgs obj)
        {
            Print("{0} objects added to chart", obj.ChartObjects.Count);
        }

        public override void Calculate(int index)
        {
        }
    }
}
