// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This sample indicator draws a green cloud for uptrends and a red cloud for downtrends on a Moving Average (MA) Crossover.  
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
    [Cloud("Fast", "Slow", Opacity = 0.2)]
    public class MACloud : Indicator
    {
        [Output("Fast", LineColor = "Green", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Fast { get; set; }
        
        [Output("Slow", LineColor = "Red", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Slow { get; set; }
        
        SimpleMovingAverage _fastMA;
        SimpleMovingAverage _slowMA;

        protected override void Initialize()
        {
            _fastMA = Indicators.SimpleMovingAverage(Bars.ClosePrices, 7);
            _slowMA = Indicators.SimpleMovingAverage(Bars.ClosePrices, 13);
        }

        public override void Calculate(int index)
        {
            Fast[index] = _fastMA.Result[index];
            Slow[index] = _slowMA.Result[index];
        }
    }
}