using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    // This sample indicator draw an ellipse on chart
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartEllipseSample : Indicator
    {
        protected override void Initialize()
        {
            Draw(0);
        }
        public override void Calculate(int index)
        {
            for (var i = 0; i < 10; i++)
            {
                Draw(i);
            }
        }
        private void Draw(int i)
        {
            var y1 = Bars.HighPrices[Chart.FirstVisibleBarIndex] > Bars.HighPrices[Chart.LastVisibleBarIndex] ? Bars.HighPrices[Chart.FirstVisibleBarIndex] : Bars.HighPrices[Chart.LastVisibleBarIndex];
            var y2 = Bars.LowPrices[Chart.FirstVisibleBarIndex] < Bars.LowPrices[Chart.LastVisibleBarIndex] ? Bars.LowPrices[Chart.FirstVisibleBarIndex] : Bars.LowPrices[Chart.LastVisibleBarIndex];
            var ellipse = Chart.DrawEllipse("ellipse"+i, Chart.FirstVisibleBarIndex, y1 + i, Chart.LastVisibleBarIndex, y2 + i, Color.FromArgb(50, Color.Red.R, Color.Red.G, Color.Red.B));
            ellipse.IsFilled = true;
        }
    }
}