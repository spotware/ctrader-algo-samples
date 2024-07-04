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
    public class BarsTickEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Bars.Tick += Bars_Tick;
        }

        private void Bars_Tick(BarsTickEventArgs obj)
        {
            Print("Last Close Price: {0} | Is new Bar: {1}", obj.Bars.LastBar.Close, obj.IsBarOpened);
        }

        public override void Calculate(int index)
        {
        }
    }
}
