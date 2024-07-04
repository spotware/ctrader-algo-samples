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
    public class ChartEllipseSample : Indicator
    {
        protected override void Initialize()
        {
            Draw();
        }

        public override void Calculate(int index)
        {
            Draw();
        }

        private void Draw()
        {
            var y1 = Bars.HighPrices[Chart.FirstVisibleBarIndex] > Bars.HighPrices[Chart.LastVisibleBarIndex] ? Bars.HighPrices[Chart.FirstVisibleBarIndex] : Bars.HighPrices[Chart.LastVisibleBarIndex];

            var y2 = Bars.LowPrices[Chart.FirstVisibleBarIndex] < Bars.LowPrices[Chart.LastVisibleBarIndex] ? Bars.LowPrices[Chart.FirstVisibleBarIndex] : Bars.LowPrices[Chart.LastVisibleBarIndex];

            var ellipse = Chart.DrawEllipse("ellipse", Chart.FirstVisibleBarIndex, y1, Chart.LastVisibleBarIndex, y2, Color.FromArgb(50, Color.Red.R, Color.Red.G, Color.Red.B));

            ellipse.IsFilled = true;
        }
    }
}
