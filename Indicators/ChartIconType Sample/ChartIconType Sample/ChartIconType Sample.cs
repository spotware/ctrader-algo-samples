using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ChartIconType
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartIconTypeSample : Indicator
    {
        [Parameter("Icon Type", DefaultValue = ChartIconType.DownArrow)]
        public ChartIconType IconType { get; set; }

        protected override void Initialize()
        {
            Chart.DrawIcon("Icon", IconType, Chart.LastVisibleBarIndex, Chart.Bars.LastBar.Low, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
