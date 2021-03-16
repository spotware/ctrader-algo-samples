using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to draw a vertical line on chart by using Chart.DrawVertical line method
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartVerticalLineSample : Indicator
    {
        protected override void Initialize()
        {
            var verticalLine = Chart.DrawVerticalLine("vertical_line", Chart.LastVisibleBarIndex, Color.Red, 2, LineStyle.DotsRare);

            verticalLine.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}
