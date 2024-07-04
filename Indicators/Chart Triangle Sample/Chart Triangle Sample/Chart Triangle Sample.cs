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
    public class ChartTriangleSample : Indicator
    {
        protected override void Initialize()
        {
            var x1 = Chart.FirstVisibleBarIndex;
            var x2 = Chart.FirstVisibleBarIndex + ((Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex) / 2);
            var x3 = Chart.LastVisibleBarIndex;

            var y1 = Bars.LowPrices[x1];
            var y2 = Bars.LowPrices.Minimum(Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex);
            var y3 = Bars.HighPrices[x3];

            var triangle = Chart.DrawTriangle("triangle_sample", x1, y1, x2, y2, x3, y3, Color.FromArgb(100, Color.Red), 2, LineStyle.Dots);

            triangle.IsInteractive = true;
            triangle.IsFilled = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}
