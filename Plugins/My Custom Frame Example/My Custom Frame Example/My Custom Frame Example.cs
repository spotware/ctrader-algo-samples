// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example plugin adds two custom frames to the charts area, displaying two different websites. 
//
//    For a detailed tutorial on creating this plugin, watch the video at: [to:do]
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
    public class MyCustomFrameExample : Plugin
    {
        WebView _cTraderWebView = new WebView();
        WebView _cTraderWebViewSite = new WebView();

        protected override void OnStart()
        {
            _cTraderWebView.Loaded += _cTraderWebView_Loaded;
            var webViewFrame = ChartManager.AddCustomFrame("Forum");
            webViewFrame.Child = _cTraderWebView;
            webViewFrame.ChartContainer.Mode = ChartMode.Multi;
            webViewFrame.Attach();

            _cTraderWebViewSite.Loaded += _cTraderWebViewSite_Loaded;
            var webViewFrameSite = ChartManager.AddCustomFrame("Site");
            webViewFrameSite.Child = _cTraderWebViewSite;
            webViewFrameSite.ChartContainer.Mode = ChartMode.Multi;
            webViewFrameSite.Attach();
        }

        private void _cTraderWebView_Loaded(WebViewLoadedEventArgs args)
        {
           _cTraderWebView.NavigateAsync("https://www.ctrader.com/forum");
        }

        private void _cTraderWebViewSite_Loaded(WebViewLoadedEventArgs args)
        {
           _cTraderWebViewSite.NavigateAsync("https://www.spotware.com");
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here
        }
    }        
}