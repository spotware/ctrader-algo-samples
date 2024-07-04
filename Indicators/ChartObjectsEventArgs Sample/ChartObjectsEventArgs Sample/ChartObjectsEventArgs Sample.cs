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
