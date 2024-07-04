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
