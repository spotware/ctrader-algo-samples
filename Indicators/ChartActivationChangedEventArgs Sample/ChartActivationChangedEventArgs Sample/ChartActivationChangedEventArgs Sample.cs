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
    public class ChartActivationChangedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            // Switch to another chart and then switch back to this indicator chart
            // Then check the logs
            Chart.Activated += Chart_Activated;
            Chart.Deactivated += Chart_Deactivated;
        }

        private void Chart_Deactivated(ChartActivationChangedEventArgs obj)
        {
            Print("Chart {0} {1} Deactivated", obj.Chart.SymbolName, obj.Chart.TimeFrame);
        }

        private void Chart_Activated(ChartActivationChangedEventArgs obj)
        {
            Print("Chart {0} {1} Activated", obj.Chart.SymbolName, obj.Chart.TimeFrame);
        }

        public override void Calculate(int index)
        {
        }
    }
}
