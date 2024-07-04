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
