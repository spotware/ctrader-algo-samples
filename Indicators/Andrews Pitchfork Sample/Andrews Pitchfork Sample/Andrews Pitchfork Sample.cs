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
    public class AndrewsPitchforkSample : Indicator
    {
        protected override void Initialize()
        {
            var barIndex1 = Chart.FirstVisibleBarIndex;
            var barIndex2 = Chart.FirstVisibleBarIndex + ((Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex) / 5);
            var barIndex3 = Chart.FirstVisibleBarIndex + ((Chart.LastVisibleBarIndex - Chart.FirstVisibleBarIndex) / 2);

            var y1 = Bars.ClosePrices[barIndex1];
            var y2 = Bars.ClosePrices[barIndex2];
            var y3 = Bars.ClosePrices[barIndex3];

            var andrewsPitchfork = Chart.DrawAndrewsPitchfork("AndrewsPitchfork", barIndex1, y1, barIndex2, y2, barIndex3, y3, Color.Red);

            andrewsPitchfork.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}
