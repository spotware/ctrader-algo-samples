using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class Fractals : Indicator
    {
        [Parameter(DefaultValue = 5, MinValue = 5, MaxValue = 200)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = 0, MaxValue = 200)]
        public int Shift { get; set; }

        [Output("Up Fractal", LineColor = "Red", PlotType = PlotType.Points, Thickness = 2)]
        public IndicatorDataSeries UpFractal { get; set; }

        [Output("Down Fractal", LineColor = "Blue", PlotType = PlotType.Points, Thickness = 2)]
        public IndicatorDataSeries DownFractal { get; set; }

        public override void Calculate(int index)
        {
            if (index < Periods)
                return;

            DrawUpFractal(index);
            DrawDownFractal(index);
        }

        private void DrawUpFractal(int index)
        {
            var periods = Periods % 2 == 0 ? Periods - 1 : Periods;
            var middleIndex = index - periods / 2;
            var middleValue = Bars.HighPrices[middleIndex];

            var up = true;

            for (var i = 0; i < periods; i++)
            {
                var currentIndex = index - i;
                if (currentIndex == middleIndex || middleValue > Bars.HighPrices[currentIndex])
                    continue;

                up = false;
                break;
            }

            var fractalIndex = middleIndex + Shift;
            if (up)
                UpFractal[fractalIndex] = middleValue;
            else
                UpFractal[fractalIndex] = double.NaN;
        }

        private void DrawDownFractal(int index)
        {
            var period = Periods % 2 == 0 ? Periods - 1 : Periods;
            var middleIndex = index - period / 2;
            var middleValue = Bars.LowPrices[middleIndex];
            var down = true;

            for (var i = 0; i < period; i++)
            {
                var currentIndex = index - i;
                if (currentIndex == middleIndex || middleValue < Bars.LowPrices[currentIndex])
                    continue;

                down = false;
                break;
            }

            var fractalIndex = middleIndex + Shift;
            if (down)
                DownFractal[fractalIndex] = middleValue;
            else
                DownFractal[fractalIndex] = double.NaN;
        }
    }
}