using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartActivationChangedEventArgs
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