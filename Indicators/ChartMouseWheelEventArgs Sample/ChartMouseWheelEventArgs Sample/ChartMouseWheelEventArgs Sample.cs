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
    public class ChartMouseWheelEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.MouseWheel += Chart_MouseWheel;
        }

        private void Chart_MouseWheel(ChartMouseWheelEventArgs obj)
        {
            var text = string.Format("Wheel Delta: {0}", obj.Delta);

            Chart.DrawStaticText("Wheel", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
