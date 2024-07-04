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
    public class EquidistantChannelSample : Indicator
    {
        protected override void Initialize()
        {
            var channel = Chart.DrawEquidistantChannel("EquidistantChannel", Chart.FirstVisibleBarIndex, Bars.LowPrices[Chart.FirstVisibleBarIndex], Chart.LastVisibleBarIndex, Bars.HighPrices[Chart.LastVisibleBarIndex], 20 * Symbol.PipSize, Color.Red);

            channel.IsInteractive = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}
