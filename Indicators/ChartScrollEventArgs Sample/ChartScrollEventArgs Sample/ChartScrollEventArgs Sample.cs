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
    public class ChartScrollEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.ScrollChanged += Chart_ScrollChanged;
        }

        private void Chart_ScrollChanged(ChartScrollEventArgs obj)
        {
            Print("Scrolled, Bars Delta : {0} | Top Y Delta: {1} | Bottom Y Delta: {2}", obj.BarsDelta, obj.TopYDelta, obj.BottomYDelta);
        }

        public override void Calculate(int index)
        {
        }
    }
}
