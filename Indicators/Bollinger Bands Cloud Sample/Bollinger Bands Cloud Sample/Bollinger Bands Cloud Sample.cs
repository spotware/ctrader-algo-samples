// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This sample indicator draws a cloud between the top and bottom bands of Bollinger Bands. 
//
//    For a detailed tutorial on creating this indicator, see this video: https://www.youtube.com/watch?v=AWqo0k0Rrag
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    [Cloud("Top", "Bottom", Opacity = 0.2)]
    
    public class BollingerBandsCloud : Indicator
    {

        [Output("Main", LineColor = "Yellow", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Main { get; set; }

        [Output("Top", LineColor = "Red", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Top { get; set; }

        [Output("Bottom", LineColor = "Red", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Bottom { get; set; }
        
        private BollingerBands _bollingerBands;

        protected override void Initialize()
        {
            _bollingerBands = Indicators.BollingerBands(Bars.ClosePrices, 20, 2, MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            Main[index] = _bollingerBands.Main[index];
            Top[index] = _bollingerBands.Top[index];
            Bottom[index] = _bollingerBands.Bottom[index];
        }
    }
}