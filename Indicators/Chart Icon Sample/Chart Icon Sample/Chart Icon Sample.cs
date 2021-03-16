using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use Chart.DrawIcon method to draw icons
    /// </summary>
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
