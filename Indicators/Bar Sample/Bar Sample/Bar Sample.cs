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
    public class BarSample : Indicator
    {
        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        {
            // You can access each bar inside Bars collection with an index either from start ([])
            // Or from end (Last)
            var currentBar = Bars[index];
            // You can access same last bar by using Bars.LastBar or Bars.Last(0)
            Print("Open: {0} | High: {1} | Low: {2} | Close: {3} | Open Time: {4:o} | Volume: {5}", currentBar.Open, currentBar.High, currentBar.Low, currentBar.Close, currentBar.OpenTime, currentBar.TickVolume);
        }
    }
}
