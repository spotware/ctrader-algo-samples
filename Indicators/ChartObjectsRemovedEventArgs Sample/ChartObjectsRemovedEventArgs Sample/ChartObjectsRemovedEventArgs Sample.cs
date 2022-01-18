using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartObjectsRemovedEventArgs
    // ChartObjectsRemovedEventArgs is derived from ChartObjectsEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartObjectsRemovedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ObjectsRemoved += Chart_ObjectsRemoved;
        }

        private void Chart_ObjectsRemoved(ChartObjectsRemovedEventArgs obj)
        {
            Print("{0} objects removed from chart", obj.ChartObjects.Count);
        }

        public override void Calculate(int index)
        {
        }
    }
}