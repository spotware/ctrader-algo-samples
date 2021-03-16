using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This indicator shows how to use the Chart.DrawFibonacciRetracement method to draw a Fibonacci Retracement
    /// </summary>
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
