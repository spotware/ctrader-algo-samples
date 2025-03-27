// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    The sample adds a new section to Trade Watch, featuring a two-by-two grid that displays information about the last known bar prices.
//
//    For a detailed tutorial on creating this plugin, watch the video at: https://youtu.be/0HB-rdwpMAY 
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.    
    [Plugin(AccessRights = AccessRights.None)]
    public class PreviousBarInfo : Plugin
    {
        // Declare variables for market data (Bars) and UI elements (Grid, TextBlocks) used to display price information.
        Bars _bars;
        Grid _grid;
        TextBlock _lowBlock;
        TextBlock _openBlock;
        TextBlock _highBlock;
        TextBlock _closeBlock;
        
        // This method is executed when the plugin starts.        
        protected override void OnStart()
        {
            var tradeWatchTab = TradeWatch.AddTab("Previous Bar Info");  // Add a new tab to TradeWatch.
            tradeWatchTab.IsSelected = true;  // Select the new tab by default.
            
            var webView = new WebView();  // Create a WebView object for embedding web content.
            tradeWatchTab.Child = webView;  // Set the WebView as the child of the TradeWatch tab.
            
            webView.NavigateAsync("https://ctrader.com/");  // Navigate to a URL in the WebView.
            
            _grid = new Grid(2, 2)  // Create a 2x2 grid for the layout of TextBlocks.
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true,
                Height = 150,
                Width = 150,
            };
            
            _bars = MarketData.GetBars(TimeFrame.Minute, "USDJPY");  // Fetch 1-minute bars for the USDJPY symbol.
            
            _lowBlock = new TextBlock  // Create a TextBlock for displaying the low price of the bar.
            {
                Text = "Low:" + _bars.LowPrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            _highBlock = new TextBlock  // Create a TextBlock for displaying the high price of the bar.
            {
                Text = "High:" + _bars.HighPrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            _closeBlock = new TextBlock  // Create a TextBlock for displaying the close price of the bar.
            {
                Text = "Close:" +_bars.ClosePrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            _openBlock = new TextBlock  // Create a TextBlock for displaying the open price of the bar.
            {
                Text = "Open:" + _bars.OpenPrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            
            // Add the TextBlocks to the grid at their respective positions.
            _grid.AddChild(_lowBlock, 0, 0);
            _grid.AddChild(_highBlock, 0, 1);
            _grid.AddChild(_openBlock, 1, 0);
            _grid.AddChild(_closeBlock, 1, 1);

            tradeWatchTab.Child = _grid;  // Set the grid as the child of the TradeWatch tab.
            
            _bars.Tick += _bars_Tick;  // Attach an event handler to update the price data every tick.

        }
        
        // Event handler to update the TextBlocks on each new tick.
        private void _bars_Tick(BarsTickEventArgs obj)
        {
            // Update the text for each TextBlock with the latest price data.
            _lowBlock.Text = "Low: " +_bars.LowPrices.LastValue.ToString();
            _highBlock.Text = "High: " +_bars.HighPrices.LastValue.ToString();
            _openBlock.Text = "Open: " +_bars.OpenPrices.LastValue.ToString();
            _closeBlock.Text = "Close: " +_bars.ClosePrices.LastValue.ToString();
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here.
        }
    }        
}
