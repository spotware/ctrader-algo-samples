// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds several commands to chart container toolbar, and uses .NET project assembly embedded resource
//    to store and load SVG icons.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Text;
using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class CommandsSample : Plugin
    {
        protected override void OnStart()
        {
            var commandWithoutResultIcon = new SvgIcon(GetSvgIconAsString("growth-icon.svg"));
            var commandWithoutResult = Commands.Add(CommandType.ChartContainerToolbar, CommandWithoutResultCallback, commandWithoutResultIcon);
            commandWithoutResult.ToolTip = "Without Result";
            
            var commandWithResultIcon = new SvgIcon(GetSvgIconAsString("innovation-creativity-icon.svg"));
            var commandWithResult = Commands.Add(CommandType.ChartContainerToolbar, CommandWithResultCallback, commandWithResultIcon);
            commandWithResult.ToolTip = "With Result";
            
            var disabledCommandIcon = new SvgIcon(GetSvgIconAsString("motor-pump-color-icon.svg"));
            var disabledCommand = Commands.Add(CommandType.ChartContainerToolbar, args => throw new InvalidOperationException("Shouldn't be executed!"), disabledCommandIcon);

            disabledCommand.ToolTip = "Disabled Command";
            disabledCommand.IsEnabled = false;
            
            var drawOnActiveChartCommand = Commands.Add(CommandType.ChartContainerToolbar, DrawOnActiveChart);
            drawOnActiveChartCommand.ToolTip = "Draws Text on active chart";
        }

        private void DrawOnActiveChart(CommandArgs obj)
        {
            if (ChartManager.ActiveFrame is not ChartFrame {Chart: var chart})
                return;

            chart.DrawStaticText(
                "CommandText",
                "Command drawing",
                VerticalAlignment.Center,
                HorizontalAlignment.Center,
                Application.DrawingColor);
        }

        private void CommandWithoutResultCallback(CommandArgs commandArgs)
        {
            if (commandArgs.Context is not ChartContainer chartContainer)
                return;
            
            MessageBox.Show(
                $"Command was executed for chart container {chartContainer.Id} which has {chartContainer.Count} charts and has {chartContainer.Mode} mode.",
                "Command Without Result");
        }
        
        private CommandResult CommandWithResultCallback(CommandArgs commandArgs)
        {
            var webView = new WebView {Width = 300, Height = 350};

            webView.Loaded += OnWebViewLoaded;
            
            return new CommandResult(webView);
        }

        private void OnWebViewLoaded(WebViewLoadedEventArgs obj) => obj.WebView.NavigateAsync("https://ctrader.com/");

        private string GetSvgIconAsString(string fileName)
        {
            var assembly = Assembly.GetAssembly(typeof(CommandsSample));
            
            var assemblyName = assembly!.GetName().Name!.Replace(' ', '_');
            
            var embeddedResourceName = $"{assemblyName}.{fileName}";
            
            using var stream = assembly.GetManifestResourceStream(embeddedResourceName);
            
            if (stream is null)
                throw new InvalidOperationException($"Resource {embeddedResourceName} not found.");
            
            using var streamReader = new StreamReader(stream, Encoding.UTF8);

            return streamReader.ReadToEnd();
        }
    }        
}