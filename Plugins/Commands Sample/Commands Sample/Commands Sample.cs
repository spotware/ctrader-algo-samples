// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds several commands to chart container toolbar, all SVG icons are stored inside SvgIcons
//    static class.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class CommandsSample : Plugin
    {
        // This method is triggered when the plugin starts.
        protected override void OnStart()
        {
            var commandWithoutResultIcon = new SvgIcon(SvgIcons.GrowthIcon);  // Create a new SvgIcon for the first command using an icon from SvgIcons class.
            var commandWithoutResult = Commands.Add(CommandType.ChartContainerToolbar, CommandWithoutResultCallback, commandWithoutResultIcon);  // Add a command to the chart container toolbar with an icon and callback.
            commandWithoutResult.ToolTip = "Without Result";  // Set the tooltip for the first command.
            
            var commandWithResultIcon = new SvgIcon(SvgIcons.InnovationCreativityIcon);  // Create a new SvgIcon for the second command using an icon from SvgIcons class.
            var commandWithResult = Commands.Add(CommandType.ChartContainerToolbar, CommandWithResultCallback, commandWithResultIcon);  // Add a command to the chart container toolbar with an icon and callback.
            commandWithResult.ToolTip = "With Result";  // Set the tooltip for the second command.
            
            var disabledCommandIcon = new SvgIcon(SvgIcons.MotorPumpColorIcon);  // Create a new SvgIcon for the disabled command using an icon from SvgIcons class.
            var disabledCommand = Commands.Add(CommandType.ChartContainerToolbar, args => throw new InvalidOperationException("Shouldn't be executed!"), disabledCommandIcon);  // Add a disabled command to the chart container toolbar that throws an exception if triggered.

            disabledCommand.ToolTip = "Disabled Command";  // Set the tooltip for the disabled command.
            disabledCommand.IsEnabled = false;  // Disable the command so it cannot be executed.
            
            var drawOnActiveChartCommand = Commands.Add(CommandType.ChartContainerToolbar, DrawOnActiveChart);  // Add a command that draws text on the active chart.
            drawOnActiveChartCommand.ToolTip = "Draws Text on active chart";  // Set the tooltip for the drawing command.
        }

        // Callback method for the draw command.
        private void DrawOnActiveChart(CommandArgs obj)
        {
            // Check if the active chart frame is valid and retrieving the chart.
            if (ChartManager.ActiveFrame is not ChartFrame {Chart: var chart})
                return;  // If no valid chart, exit the method.

            // Draw static text on the active chart at the center.
            chart.DrawStaticText(
                "CommandText",
                "Command drawing",
                VerticalAlignment.Center,
                HorizontalAlignment.Center,
                Application.DrawingColor);
        }

        // Callback method for the first command.
        private void CommandWithoutResultCallback(CommandArgs commandArgs)
        {
            // Check if the context is a valid ChartContainer.
            if (commandArgs.Context is not ChartContainer chartContainer)
                return;  // If not a valid chart container, exit the method.
            
            // Showing a message box with details about the chart container.
            MessageBox.Show(
                $"Command was executed for chart container {chartContainer.Id} which has {chartContainer.Count} charts and has {chartContainer.Mode} mode.",
                "Command Without Result");
        }
        
        // Callback method for the second command.
        private CommandResult CommandWithResultCallback(CommandArgs commandArgs)
        {
            var webView = new WebView {Width = 300, Height = 350};  // Create a WebView to display a webpage in the command result.

            webView.Loaded += OnWebViewLoaded;  // Subscribe to the WebView Loaded event to handle when the page finishes loading.
            
            return new CommandResult(webView);  // Return the WebView as the command result.
        }

        // Event handler for when the WebView is loaded.
        private void OnWebViewLoaded(WebViewLoadedEventArgs obj) => obj.WebView.NavigateAsync("https://ctrader.com/");  // Navigate to the cTrader website once the WebView has finished loading.
    }        
}
