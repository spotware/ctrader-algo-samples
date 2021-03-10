using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Chart.DrawFibonacciFan method to draw a Fibonacci Fan
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FibonacciFanSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.DrawFibonacciFan("Fan", Chart.FirstVisibleBarIndex, Bars.ClosePrices[Chart.FirstVisibleBarIndex],
                Chart.LastVisibleBarIndex, Bars.ClosePrices[Chart.LastVisibleBarIndex], Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}