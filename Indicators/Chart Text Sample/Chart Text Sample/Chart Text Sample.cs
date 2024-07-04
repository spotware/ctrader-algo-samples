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
