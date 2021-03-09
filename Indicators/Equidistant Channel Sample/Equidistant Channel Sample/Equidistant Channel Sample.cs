using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// A sample indicator for showing how to use Chart.DrawEquidistantChannel method
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class EquidistantChannelSample : Indicator
    {
        protected override void Initialize()
        {
            var channel = Chart.DrawEquidistantChannel("EquidistantChannel", Chart.FirstVisibleBarIndex,
                Bars.LowPrices[Chart.FirstVisibleBarIndex], Chart.LastVisibleBarIndex, Bars.HighPrices[Chart.LastVisibleBarIndex],
                20 * Symbol.PipSize, Color.Red);

            channel.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}