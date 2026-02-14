// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or 
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds a new custom window containing a 'Add Take Profit' button,
//    which adds take profit to open positions when clicked.
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
    public class CustomWindowPlugin : Plugin

    {
        private Button _buttonAddTakeProfit;  // Define button control for adding take profit.
        private Window _window;  // Define custom window to display the button.

        // This method is executed when the plugin starts.
        protected override void OnStart()
        {
            // Initialise the 'Add Take Profit' button with properties.
            _buttonAddTakeProfit = new Button
            {
                BackgroundColor = Color.SeaGreen,
                Height = 50,
                Text = "Add Take Profit"
            };

            // Attach the click event handler for the button.
            _buttonAddTakeProfit.Click += _buttonAddTakeProfit_Click;

            // Initialise the custom window with properties.
            _window = new Window
            {
                Height = 150,
                Width = 150,
                Padding = new Thickness(5, 10, 10, 5)
            };

            // Add the button as the child control of the window.
            _window.Child = _buttonAddTakeProfit;
            
            // Display the custom window.
            _window.Show();
        }

        // This method handles the button click event.
        private void _buttonAddTakeProfit_Click(ButtonClickEventArgs args)
        {
            // Loop through all open positions.
            foreach (var position in Positions)
            {
                // Check if the position has no take profit set.
                if (position.TakeProfit is null)
                {
                    position.ModifyTakeProfitPips(20);  // Modify the position to add a take profit of 20 pips.
                }
            }
        }        

        protected override void OnStop()
        {
            // Handle Plugin stop here.
        }
    }        
}
