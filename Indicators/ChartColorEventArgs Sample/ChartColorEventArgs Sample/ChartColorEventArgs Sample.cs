using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartColorEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartColorEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ColorsChanged += Chart_ColorsChanged;
        }

        private void Chart_ColorsChanged(ChartColorEventArgs obj)
        {
            Print("Chart {0} {1} Color changed", obj.Chart.SymbolName, obj.Chart.TimeFrame);
        }

        public override void Calculate(int index)
        {
        }
    }
}