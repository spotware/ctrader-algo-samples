// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example plugin adds a new section to Trade Watch, featuring a two-by-two grid that displays information about the last known bar prices.
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
    [Plugin(AccessRights = AccessRights.None)]
    public class PreviousBarInfo : Plugin
    {
        
        Bars _bars;
        Grid _grid;
        TextBlock _lowBlock;
        TextBlock _openBlock;
        TextBlock _highBlock;
        TextBlock _closeBlock;
        
        protected override void OnStart()
        {
            var tradeWatchTab = TradeWatch.AddTab("Previous Bar Info");
            tradeWatchTab.IsSelected = true;
            
            var webView = new WebView();                        
            tradeWatchTab.Child = webView;
            
            webView.NavigateAsync("https://ctrader.com/");
            
            _grid = new Grid(2, 2) 
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true,
                Height = 150,
                Width = 150,
            };
            
            _bars = MarketData.GetBars(TimeFrame.Minute, "USDJPY");
            
            _lowBlock = new TextBlock 
            {
                Text = "Low:" + _bars.LowPrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            _highBlock = new TextBlock 
            {
                Text = "High:" + _bars.HighPrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            _closeBlock = new TextBlock 
            {
                Text = "Close:" +_bars.ClosePrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            _openBlock = new TextBlock 
            {
                Text = "Open:" + _bars.OpenPrices.LastValue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            
            _grid.AddChild(_lowBlock, 0, 0);
            _grid.AddChild(_highBlock, 0, 1);
            _grid.AddChild(_openBlock, 1, 0);
            _grid.AddChild(_closeBlock, 1, 1);

            tradeWatchTab.Child = _grid;
            
            _bars.Tick += _bars_Tick;


        }
        
        private void _bars_Tick(BarsTickEventArgs obj)
        {
            _lowBlock.Text = "Low: " +_bars.LowPrices.LastValue.ToString();
            _highBlock.Text = "High: " +_bars.HighPrices.LastValue.ToString();
            _openBlock.Text = "Open: " +_bars.OpenPrices.LastValue.ToString();
            _closeBlock.Text = "Close: " +_bars.ClosePrices.LastValue.ToString();
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here
        }
    }        
}