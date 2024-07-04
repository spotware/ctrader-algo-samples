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
