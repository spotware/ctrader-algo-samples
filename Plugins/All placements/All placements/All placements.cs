// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
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
    public class Allplacements : Plugin
    {
        const string WebViewUrl = "https://ctrader.com";

        protected override void OnStart()
        {
            var icon = new SvgIcon(@"
<svg class='w-6 h-6 text-gray-800 dark:text-white' aria-hidden='true' xmlns='http://www.w3.org/2000/svg' width='15' height='15' fill='none' viewBox='0 0 24 24'>
  <path stroke='#BFBFBF' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M11 6.5h2M11 18h2m-7-5v-2m12 2v-2M5 8h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0 12h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm12 0h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0-12h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Z'/>
</svg>
");
            Commands.Add(CommandType.ChartContainerToolbar, OnIconClicked, icon);
        }

        private CommandResult OnIconClicked(CommandArgs args)
        {
            var buttonStyle = new Style();
            buttonStyle.Set(ControlProperty.Margin, new Thickness(0, 5, 0, 0));
            buttonStyle.Set(ControlProperty.Width, 150);
            var stackPanel = new StackPanel();

            var showMessageBoxButton = new Button { Text = "show MessageBox", Style = buttonStyle };
            showMessageBoxButton.Click += args =>
            {
                MessageBox.Show("Some message", "Caption", MessageBoxButton.YesNo);
            };
            stackPanel.AddChild(showMessageBoxButton);

            var showCustomWindowButton = new Button { Text = "show Custom Window", Style = buttonStyle };
            showCustomWindowButton.Click += args =>
            {
                var window = new Window();
                var webView = new WebView();
                window.Child = webView;

                window.Show();
                webView.NavigateAsync(WebViewUrl);
            };
            stackPanel.AddChild(showCustomWindowButton);

            var blockCounter = 1;
            var addAspBlockButton = new Button { Text = "add ASP Block", Style = buttonStyle };
            addAspBlockButton.Click += args =>
            {
                var newBlock = Asp.SymbolTab.AddBlock("One more block " + blockCounter);
                newBlock.IsExpanded = true;
                newBlock.Height = 600;
                blockCounter++;

                var webView = new WebView();
                newBlock.Child = webView;
                webView.NavigateAsync(WebViewUrl);
            };
            stackPanel.AddChild(addAspBlockButton);

            var aspTabCounter = 1;
            var addAspTabButton = new Button { Text = "add ASP Tab", Style = buttonStyle };
            addAspTabButton.Click += args =>
            {
                var tab = Asp.AddTab("ASP tab " + aspTabCounter);
                tab.Index = aspTabCounter;
                aspTabCounter++;

                var webView = new WebView();
                tab.Child = webView;
                webView.NavigateAsync(WebViewUrl);
            };
            stackPanel.AddChild(addAspTabButton);

            var tradewatchTabCounter = 1;
            var addTradeWatchTabButton = new Button { Text = "add TradeWatch Tab", Style = buttonStyle };
            addTradeWatchTabButton.Click += args =>
            {
                var tab = TradeWatch.AddTab("Tab " + tradewatchTabCounter);
                tab.Index = tradewatchTabCounter;
                tradewatchTabCounter++;
                tab.IsSelected = true;

                var webView = new WebView();
                tab.Child = webView;
                webView.NavigateAsync(WebViewUrl);
            };
            stackPanel.AddChild(addTradeWatchTabButton);

            var addCustomFrameButton = new Button { Text = "add Custom Frame", Style = buttonStyle };
            var customFrameCounter = 1;
            addCustomFrameButton.Click += args =>
            {
                var frame = ChartManager.AddCustomFrame("Custom Frame " + customFrameCounter);
                customFrameCounter++;

                var webView = new WebView();
                frame.Child = webView;
                webView.NavigateAsync(WebViewUrl);
            };
            stackPanel.AddChild(addCustomFrameButton);

            var customizeActiveChartButton = new Button { Text = "customize Active Chart", Style = buttonStyle };
            customizeActiveChartButton.Click += args =>
            {
                var activeFrame = ChartManager.ActiveFrame;
                if (activeFrame is ChartFrame chartFrame)
                {
                    var chart = chartFrame.Chart;
                    chart.ColorSettings.BackgroundColor = Color.DarkBlue;
                    chart.DisplaySettings.TickVolume = false;
                    chart.ZoomLevel = 10;
                }
            };
            stackPanel.AddChild(customizeActiveChartButton);

            var border = new Border();
            border.Padding = 5;
            border.BackgroundColor = "#1A1A1A";
            border.CornerRadius = 3;
            border.BorderThickness = 1;
            border.BorderColor = "525252";
            border.Child = stackPanel;
            border.Width = 170;
            border.Height = 190;
            return new CommandResult(border);
        }
    }
}