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
    public class ChartSizeEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.SizeChanged += Chart_SizeChanged;
        }

        private void Chart_SizeChanged(ChartSizeEventArgs obj)
        {
            Chart.DrawStaticText("size", "Size Changed", VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
