using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartObjectsEventArgs
    // All chart objects related events args are derived from ChartObjectsEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartObjectsEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ObjectsRemoved += Chart_ObjectsRemoved;
        }

        private void Chart_ObjectsRemoved(ChartObjectsRemovedEventArgs obj)
        {
            var chartObjectsEventArgs = obj as ChartObjectsEventArgs;

            Print("{0} objects removed from chart", chartObjectsEventArgs.ChartObjects.Count);
        }

        public override void Calculate(int index)
        {
        }
    }
}
