using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// Thios sample shows how to use Chart.DrawText method to draw text
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartTextSample : Indicator
    {
        protected override void Initialize()
        {
            for (int iBarIndex = Chart.FirstVisibleBarIndex; iBarIndex <= Chart.LastVisibleBarIndex; iBarIndex++)
            {
                string text;

                double y;

                Color color;

                if (Bars.ClosePrices[iBarIndex] > Bars.OpenPrices[iBarIndex])
                {
                    text = "U";
                    y = Bars.LowPrices[iBarIndex];
                    color = Color.Green;
                }
                else
                {
                    text = "D";
                    y = Bars.HighPrices[iBarIndex];
                    color = Color.Red;
                }

                Chart.DrawText("Text_" + iBarIndex, text, iBarIndex, y, color);
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}