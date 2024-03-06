// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a custom tab into the TradeWatch panel. The plugin also opens an H1 chart
//    for EURUSD on start. Whenever the user hovers the mouse cursor over the chart, the TextBlock
//    inside the new TradeWatchTab displays HLOC information about the bar over which the cursor is
//    currently hovering. This is achieved via the XToBarIndex() method.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class CoordinatesConversionSample : Plugin
    {

        // Creating a variable to store the bar index
        // and the chart; also declaring the TextBlock
        private int _hoverIndex;
        private TextBlock _priceInfoTextBlock;
        private Chart _eurusdChart;

        protected override void OnStart()
        {
            // Initialising the TextBlock and setting
            // its parameters
            _priceInfoTextBlock = new TextBlock
            {
                TextAlignment = TextAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Initialising...",
                FontSize = 26,
                FontWeight = FontWeight.Bold,
                
            };
            
            // Adding a new tab into TradeWatch and
            // setting the TextBlock as its child
            var tradeWatchTab = TradeWatch.AddTab("EURUSD Hover Price");
            tradeWatchTab.Child = _priceInfoTextBlock;
            
            // Opening a new ChartFrame and storing its Chart
            var eurusdChartFrame = ChartManager.AddChartFrame("EURUSD", TimeFrame.Hour);
            _eurusdChart = eurusdChartFrame.Chart;
            
            // Handling mouse movement events
            _eurusdChart.MouseEnter += EurusdChartOnMouseHover;
            _eurusdChart.MouseLeave += EurusdChartOnMouseLeave;
            _eurusdChart.MouseMove += EurusdChartOnMouseHover;
        }

        private void EurusdChartOnMouseHover(ChartMouseEventArgs obj)
        {
            // Attaining the Bars for EURUSD and determining
            // the bar index over which the mouse cursor is hovering
            var bars = MarketData.GetBars(TimeFrame.Hour, "EURUSD");
            _hoverIndex = Convert.ToInt32(_eurusdChart.XToBarIndex(obj.MouseX));
            
            // Updating the text inside the TextBlock
            _priceInfoTextBlock.Text = @$"EURUSD Price at {_hoverIndex}:
                                         {bars[_hoverIndex].OpenTime}
                                         {bars[_hoverIndex].Open}
                                         {bars[_hoverIndex].Close}
                                         {bars[_hoverIndex].High}
                                         {bars[_hoverIndex].Low}";
        }

        // Stop s
        private void EurusdChartOnMouseLeave(ChartMouseEventArgs obj)
        {
            // Displaying special text when the mouse cursor
            // leaves the chart
            _priceInfoTextBlock.Text = "EURUSD Price: Unavavailable";
        }
        
    }        
}