// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The sample demonstrates how to integrate and customise various components in cTrader, such as 
//    adding commands to toolbars, creating Active Symbol Panel (ASP) blocks, tabs and custom frames. 
//    It also demonstrates customising the active chart appearance and functionality.
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
    public class Allplacements : Plugin
    {
        const string WebViewUrl = "https://ctrader.com";  // Define the URL to be used in WebView instances.

        // This method is triggered when the plugin starts.
        protected override void OnStart()
        {
            var icon = new SvgIcon(@"
<svg class='w-6 h-6 text-gray-800 dark:text-white' aria-hidden='true' xmlns='http://www.w3.org/2000/svg' width='15' height='15' fill='none' viewBox='0 0 24 24'>
  <path stroke='#BFBFBF' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M11 6.5h2M11 18h2m-7-5v-2m12 2v-2M5 8h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0 12h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm12 0h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0-12h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Z'/>
</svg>
");  // Define an icon to be used in the toolbar.

            Commands.Add(CommandType.ChartContainerToolbar, OnIconClicked, icon);  // Add the icon to the chart toolbar, which will trigger the OnIconClicked method when clicked.
        }

        // This method is triggered when the toolbar icon is clicked.
        private CommandResult OnIconClicked(CommandArgs args)
        {
            var buttonStyle = new Style();  // Create a new style for buttons.
            buttonStyle.Set(ControlProperty.Margin, new Thickness(0, 5, 0, 0));  // Set the margin for the button.
            buttonStyle.Set(ControlProperty.Width, 150);  // Set the button width.
            var stackPanel = new StackPanel();  // Create a StackPanel to stack UI elements vertically.

            // Define the first button that shows a message box when clicked.
            var showMessageBoxButton = new Button { Text = "show MessageBox", Style = buttonStyle };
            showMessageBoxButton.Click += args =>  // Define the event handler for button click.
            {
                MessageBox.Show("Some message", "Caption", MessageBoxButton.YesNo);  // Show a message box with a Yes/No prompt.
            };
            stackPanel.AddChild(showMessageBoxButton);  // Add the button to the stack panel.

            // Define the second button that opens a custom window with a WebView.
            var showCustomWindowButton = new Button { Text = "show Custom Window", Style = buttonStyle };
            showCustomWindowButton.Click += args =>  // Define the event handler for button click.
            {
                var window = new Window();  // Create a new window.
                var webView = new WebView();  // Create a WebView control inside the window.
                window.Child = webView;  // Set the WebView as the child of the window.

                window.Show();  // Show the window.
                webView.NavigateAsync(WebViewUrl);  // Navigate to the URL in the WebView.
            };
            stackPanel.AddChild(showCustomWindowButton);  // Add the button to the stack panel.

            // Define the parameters of the first option in the drop-down list ("add ASP Block").
            var blockCounter = 1;  // Counter for adding ASP blocks dynamically.
            var addAspBlockButton = new Button { Text = "add ASP Block", Style = buttonStyle };  // Button labeled "add ASP Block" with the defined button style.
            addAspBlockButton.Click += args =>  // Define the event handler for adding a new ASP block.
            {
                var newBlock = Asp.SymbolTab.AddBlock("One more block " + blockCounter);  // Add a new block in the symbol tab.
                newBlock.IsExpanded = true;  // Expand the block to show content.
                newBlock.Height = 600;  // Set the block height.
                blockCounter++;  // Increment the block counter.

                var webView = new WebView();  // Create a WebView to display in the new block.
                newBlock.Child = webView;  // Add the WebView as the child of the new block.
                webView.NavigateAsync(WebViewUrl);  // Navigate to the specified URL in the WebView.
            };
            stackPanel.AddChild(addAspBlockButton);  // Add the button to the stack panel.

            // Define the parameters of the next option in the drop-down list ("add ASP Tab").
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

            // Define the parameters of the next option in the drop-down list ("add TradeWatch Tab").
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

            // Define the parameters of the next option in the drop-down list ("add Custom Frame").
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

            // Define the parameters of the next option in the drop-down list ("customize Active Chart").
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

            var border = new Border();  // Create a border container for the stack panel.
            border.Padding = 5;  // Add padding inside the border.
            border.BackgroundColor = "#1A1A1A";  // Set the background color of the border.
            border.CornerRadius = 3;  // Round the corners of the border with a radius of 3.
            border.BorderThickness = 1;  // Set the thickness of the border to 1.
            border.BorderColor = "525252";  // Set the color of the border to a gray shade.
            border.Child = stackPanel;  // Assign the stack panel as the child of the border.
            border.Width = 170;  // Set the width of the border.
            border.Height = 190;  // Set the height of the border.
            return new CommandResult(border);  // Return the border as the result of the command, completing the customisation UI.
        }
    }
}
