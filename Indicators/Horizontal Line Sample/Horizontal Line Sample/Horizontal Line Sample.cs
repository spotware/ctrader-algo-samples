using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use Chart.DrawHorizontalLine method to draw an horizontal line
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class HorizontalLineSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.DrawHorizontalLine("horizontalLine", Bars.ClosePrices.LastValue, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}