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
