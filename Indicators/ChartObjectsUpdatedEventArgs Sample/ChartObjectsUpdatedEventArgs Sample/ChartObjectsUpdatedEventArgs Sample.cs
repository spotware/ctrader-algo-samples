using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartObjectsUpdatedEventArgs
    // Draw an object, and then modify it
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartObjectsUpdatedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ObjectsUpdated += Chart_ObjectsUpdated;
        }

        private void Chart_ObjectsUpdated(ChartObjectsUpdatedEventArgs obj)
        {
            Print("Updated objects #: {0}", obj.ChartObjects.Count);
        }

        public override void Calculate(int index)
        {
        }
    }
}
