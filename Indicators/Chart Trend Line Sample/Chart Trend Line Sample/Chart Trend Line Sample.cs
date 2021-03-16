using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Chart.DrawTrendLine to draw a trend line on chart
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartTrendLineSample : Indicator
    {
        protected override void Initialize()
        {
            var trendLine = Chart.DrawTrendLine("trendLine", Chart.FirstVisibleBarIndex, Bars.LowPrices[Chart.FirstVisibleBarIndex], Chart.LastVisibleBarIndex, Bars.HighPrices[Chart.LastVisibleBarIndex], Color.Red, 2, LineStyle.Dots);

            trendLine.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}
