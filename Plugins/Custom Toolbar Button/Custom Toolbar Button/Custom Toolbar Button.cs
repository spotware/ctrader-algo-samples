// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds a new button with a pop-up menu to the chart toolbar, featuring
//    a 'Close All Positions' button that closes open positions when clicked.
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
    public class CustomToolbarButton : Plugin
    {
        // This method is executed when the plugin starts.
        protected override void OnStart()
        {
            var icon = new SvgIcon(@"<svg class='w-6 h-6 text-gray-800 dark:text-white' aria-hidden='true' xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 24 24'>
    <path stroke='#BFBFBF' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M11 6.5h2M11 18h2m-7-5v-2m12 2v-2M5 8h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0 12h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1H5a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm12 0h2a1 1 0 0 0 1-1v-2a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Zm0-12h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1h-2a1 1 0 0 0-1 1v2a1 1 0 0 0 1 1Z'/>
    </svg>");  // Define an icon to be used in the toolbar.

            // Add a command to the chart toolbar to open positions.
            var command = Commands.Add(CommandType.ChartContainerToolbar, OpenPositions, icon);
            command.ToolTip = "Open Positions";

            // Add another command for the 'Close All Positions' functionality.
            Commands.Add(CommandType.ChartContainerToolbar, CloseAllPositions, icon);
        }

        // This method creates a pop-up menu containing a 'Close All Positions' button to close all open positions.
        private CommandResult CloseAllPositions (CommandArgs args)
        {
            // Create a button style for uniform appearance.
            var buttonStyle = new Style();
            buttonStyle.Set(ControlProperty.Margin, new Thickness(0, 5, 0, 0));  // Set button margins.
            buttonStyle.Set(ControlProperty.Width, 150);  // Set button width.

            // Define the 'Close All Positions' button.
            var closePositionsButton = new Button
            {
                Text = "Close All Positions", 
                Style = buttonStyle  
            };

            // Handle button click to close all open positions.
            closePositionsButton.Click += args =>
            {
                // Iterate through all positions.
                foreach (var position in Positions)
                {
                    position.Close();  // Close each position.
                }
            };

            // Create a stack panel to contain the button.
            var stackPanel = new StackPanel();
            stackPanel.AddChild(closePositionsButton);  // Add the button to the panel.

            // Return the panel as the result for the command.
            return new CommandResult(stackPanel);
        }

        // This method opens predefined buy positions for selected currency pairs.
        private void OpenPositions(CommandArgs args) 
        {
            ExecuteMarketOrder(TradeType.Buy, "EURUSD", 1000);  // Open a buy position for EUR/USD with a volume of 1000.
            ExecuteMarketOrder(TradeType.Buy, "USDJPY", 1000);  // Open a buy position for USD/JPY with a volume of 1000.
            ExecuteMarketOrder(TradeType.Buy, "EURGBP", 1000);  // Open a buy position for EUR/GBP with a volume of 1000.
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here.
        }
    }        
}
