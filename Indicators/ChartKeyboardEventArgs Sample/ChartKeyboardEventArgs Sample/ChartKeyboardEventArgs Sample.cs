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
    public class ChartKeyboardEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.KeyDown += Chart_KeyDown;
        }

        private void Chart_KeyDown(ChartKeyboardEventArgs obj)
        {
            Print("Key: {0} | Modifier Key: {1}", obj.Key, obj.Modifiers);
        }

        public override void Calculate(int index)
        {
        }
    }
}
