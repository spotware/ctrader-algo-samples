using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use a chart Fibonacci Retracement levels property to modify the fibonacci levels
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FibonacciLevelSample : Indicator
    {
        protected override void Initialize()
        {
            var period = Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex;

            var max = Bars.HighPrices.Maximum(period);
            var min = Bars.LowPrices.Minimum(period);

            var fibonacciRetracement = Chart.DrawFibonacciRetracement("FibonacciRetracement", Chart.FirstVisibleBarIndex, max, Chart.LastVisibleBarIndex, min, Color.Red);

            foreach (var level in fibonacciRetracement.FibonacciLevels)
            {
                Print(level.PercentLevel);

                if (level.PercentLevel > 62)
                    level.IsVisible = false;
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}
