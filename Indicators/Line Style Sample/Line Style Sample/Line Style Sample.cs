using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use different line styles
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class LineStyleSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.DrawVerticalLine("Dots", Chart.LastVisibleBarIndex, Color.Red, 3, LineStyle.Dots);
            Chart.DrawVerticalLine("DotsRare", Chart.LastVisibleBarIndex - 2, Color.Yellow, 3, LineStyle.DotsRare);
            Chart.DrawVerticalLine("DotsVeryRare", Chart.LastVisibleBarIndex - 4, Color.Green, 3, LineStyle.DotsVeryRare);
            Chart.DrawVerticalLine("Lines", Chart.LastVisibleBarIndex - 6, Color.Blue, 3, LineStyle.Lines);
            Chart.DrawVerticalLine("LinesDots", Chart.LastVisibleBarIndex - 8, Color.Magenta, 3, LineStyle.LinesDots);
            Chart.DrawVerticalLine("Solid", Chart.LastVisibleBarIndex - 10, Color.Brown, 3, LineStyle.Solid);
        }

        public override void Calculate(int index)
        {
        }
    }
}
