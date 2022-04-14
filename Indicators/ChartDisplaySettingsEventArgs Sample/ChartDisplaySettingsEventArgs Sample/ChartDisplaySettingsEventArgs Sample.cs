using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartDisplaySettingsEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartDisplaySettingsEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.DisplaySettingsChanged += Chart_DisplaySettingsChanged;
        }

        private void Chart_DisplaySettingsChanged(ChartDisplaySettingsEventArgs obj)
        {
            Print("Chart {0} {1} Display settings changed", obj.Chart.SymbolName, obj.Chart.TimeFrame);
        }

        public override void Calculate(int index)
        {
        }
    }
}
