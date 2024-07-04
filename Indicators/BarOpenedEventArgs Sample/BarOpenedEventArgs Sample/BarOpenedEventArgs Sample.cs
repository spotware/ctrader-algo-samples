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
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class BarOpenedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Bars.BarOpened += Bars_BarOpened;
        }

        private void Bars_BarOpened(BarOpenedEventArgs obj)
        {
            var newOpendBar = obj.Bars.LastBar;
            // Or you can use obj.Bars[Bars.Count - 1] or obj.Bars.Last(0)
            var closedBar = obj.Bars.Last(1);
            // Or you can use obj.Bars[Bars.Count - 2]
        }

        public override void Calculate(int index)
        {
        }
    }
}
