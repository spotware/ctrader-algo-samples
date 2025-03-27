// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds two custom frames to the charts area, displaying two different websites.
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
    public class MyCustomFrameExample : Plugin
    {
        WebView _cTraderWebView = new WebView();  // WebView for displaying the cTrader forum website.
        WebView _cTraderWebViewSite = new WebView();  // WebView for displaying the Spotware website.

        // This method is executed when the plugin starts.
        protected override void OnStart()
        {
            // Handle the Loaded event when the first WebView has finished loading.
            _cTraderWebView.Loaded += _cTraderWebView_Loaded;
            
            // Create a custom frame named "Forum" and set the WebView as its child.
            var webViewFrame = ChartManager.AddCustomFrame("Forum");  // Add a custom frame to the chart container.
            webViewFrame.Child = _cTraderWebView;  // Set the first WebView as the child of the frame.
            webViewFrame.ChartContainer.Mode = ChartMode.Multi;  // Set chart container mode to Multi (supports multiple views).
            webViewFrame.Attach();  // Attach the custom frame to the chart.

            // Handle the Loaded event when the second WebView has finished loading.
            _cTraderWebViewSite.Loaded += _cTraderWebViewSite_Loaded;

            // Create a custom frame named "Site" and set the second WebView as its child.
            var webViewFrameSite = ChartManager.AddCustomFrame("Site");
            webViewFrameSite.Child = _cTraderWebViewSite;
            webViewFrameSite.ChartContainer.Mode = ChartMode.Multi;
            webViewFrameSite.Attach();
        }

        // Event handler for the first WebView Loaded event.
        private void _cTraderWebView_Loaded(WebViewLoadedEventArgs args)
        {
           // Navigate to the cTrader forum website once the WebView has finished loading.
           _cTraderWebView.NavigateAsync("https://www.ctrader.com/forum");
        }

        // Event handler for the second WebView Loaded event.
        private void _cTraderWebViewSite_Loaded(WebViewLoadedEventArgs args)
        {
           // Navigate to the Spotware website once the WebView has finished loading.
           _cTraderWebViewSite.NavigateAsync("https://www.spotware.com");
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here.
        }
    }        
}
