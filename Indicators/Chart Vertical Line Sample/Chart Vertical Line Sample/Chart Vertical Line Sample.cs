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
