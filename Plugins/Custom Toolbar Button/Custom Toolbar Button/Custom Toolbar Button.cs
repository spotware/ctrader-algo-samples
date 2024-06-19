// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example plugin adds a new button with a pop-up menu to the Chart Toolbar, featuring a 'Close All Positions' button that closes open positions when clicked. 
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
    public class CustomToolbarButton : Plugin
    {
        protected override void OnStart()
        {
            var icon = new SvgIcon(@"<svg class='w-6 h-6 text-gray-800 dark:text-white' aria-hidden='true' xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24'>
    <path stroke='#BFBFBF' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M11 6.5h2M11 18h2m-7-5v-2m12 2v-2M5 8h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0 12h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm12 0h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0-12h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Z'/>
    </svg>");

            var command = Commands.Add(CommandType.ChartContainerToolbar, OpenPositions, icon);
            command.ToolTip = "Open Positions";

            Commands.Add(CommandType.ChartContainerToolbar, CloseAllPositions, icon);
        }

        private CommandResult CloseAllPositions (CommandArgs args)
        {
            var buttonStyle = new Style();

            buttonStyle.Set(ControlProperty.Margin, new Thickness(0, 5, 0, 0));
            buttonStyle.Set(ControlProperty.Width, 150);

            var closePositionsButton = new Button
            {
                Text = "Close All Positions", 
                Style = buttonStyle  
            };

            closePositionsButton.Click += args =>
            {
                foreach (var position in Positions)
                {
                    position.Close();
                }
            };

            var stackPanel = new StackPanel();
            stackPanel.AddChild(closePositionsButton);

            return new CommandResult(stackPanel);
        }

        private void OpenPositions(CommandArgs args) 
        {
            ExecuteMarketOrder(TradeType.Buy, "EURUSD", 1000); 
            ExecuteMarketOrder(TradeType.Buy, "USDJPY", 1000); 
            ExecuteMarketOrder(TradeType.Buy, "EURGBP", 1000);  
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here
        }
    }        
}