using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// A sample indicator for showing how to use the Chart.DrawFibonacciExpansion method
    /// /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FibonacciExpansionSample : Indicator
    {
        protected override void Initialize()
        {
            var period = Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex;

            var fibonacciExpansion = Chart.DrawFibonacciExpansion("fibonacciExpansion", Chart.FirstVisibleBarIndex,
                Bars.LowPrices[Chart.FirstVisibleBarIndex], Chart.FirstVisibleBarIndex, Bars.LowPrices.Minimum(period),
                Chart.LastVisibleBarIndex, Bars.HighPrices.Maximum(period), Color.Red);

            fibonacciExpansion.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}