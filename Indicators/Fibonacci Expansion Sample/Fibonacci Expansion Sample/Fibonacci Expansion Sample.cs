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
    public class FibonacciExpansionSample : Indicator
    {
        protected override void Initialize()
        {
            var period = Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex;

            var fibonacciExpansion = Chart.DrawFibonacciExpansion("fibonacciExpansion", Chart.FirstVisibleBarIndex, Bars.LowPrices[Chart.FirstVisibleBarIndex], Chart.FirstVisibleBarIndex, Bars.LowPrices.Minimum(period), Chart.LastVisibleBarIndex, Bars.HighPrices.Maximum(period), Color.Red);

            fibonacciExpansion.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}
