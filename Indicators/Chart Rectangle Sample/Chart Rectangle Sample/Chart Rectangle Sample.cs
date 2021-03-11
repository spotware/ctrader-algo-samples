using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Chart.DrawRectangle method to draw a rectangle
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartRectangleSample : Indicator
    {
        protected override void Initialize()
        {
            var period = Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex;

            var rectangle = Chart.DrawRectangle("rectangle_sample", Chart.FirstVisibleBarIndex, Bars.LowPrices.Minimum(period), Chart.LastVisibleBarIndex, Bars.HighPrices.Maximum(period), Color.FromArgb(100, Color.Red));

            rectangle.IsFilled = true;
            rectangle.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}