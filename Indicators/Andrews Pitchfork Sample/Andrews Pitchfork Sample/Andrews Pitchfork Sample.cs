using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This indicator shows how to draw an Andrews Pitchfork, not the best way to do it but its just a guide
    /// </summary>
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