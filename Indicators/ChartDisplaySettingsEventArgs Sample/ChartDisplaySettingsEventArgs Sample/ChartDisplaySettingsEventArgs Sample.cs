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
