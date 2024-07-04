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
    public class FibonacciRetracementSample : Indicator
    {
        protected override void Initialize()
        {
            var period = Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex;

            var max = Bars.HighPrices.Maximum(period);
            var min = Bars.LowPrices.Minimum(period);

            Chart.DrawFibonacciRetracement("FibonacciRetracement", Chart.FirstVisibleBarIndex, max, Chart.LastVisibleBarIndex, min, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
