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
    public class ChartIconSample : Indicator
    {
        protected override void Initialize()
        {
            for (int i = Chart.FirstVisibleBarIndex; i <= Chart.LastVisibleBarIndex; i++)
            {
                var iconName = string.Format("Icon_{0}", i);

                if (Bars.ClosePrices[i] > Bars.OpenPrices[i])
                {
                    Chart.DrawIcon(iconName, ChartIconType.UpArrow, i, Bars.LowPrices[i], Color.Green);
                }
                else
                {
                    Chart.DrawIcon(iconName, ChartIconType.DownArrow, i, Bars.HighPrices[i], Color.Red);
                }
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}
