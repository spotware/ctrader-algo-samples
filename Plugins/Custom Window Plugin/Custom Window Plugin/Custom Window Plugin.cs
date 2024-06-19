// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example plugin adds a new custom window containing a 'Add Take Profit' button, which adds Take Profit to open positions when clicked.
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
    public class CustomWindowPlugin : Plugin

    {
        private Button _buttonAddTakeProfit;
        private Window _window;

        protected override void OnStart()
        {
            _buttonAddTakeProfit = new Button
            {
                BackgroundColor = Color.SeaGreen,
                Height = 50,
                Text = "Add Take Profit"
            };

            _buttonAddTakeProfit.Click += _buttonAddTakeProfit_Click;

            _window = new Window
            {
                Height = 150,
                Width = 150,
                Padding = new Thickness(5, 10, 10, 5)
            };

            _window.Child = _buttonAddTakeProfit;
            _window.Show();
        }

        private void _buttonAddTakeProfit_Click(ButtonClickEventArgs args)
        {
            foreach (var position in Positions)
            {
                if (position.TakeProfit is null)
                {
                    position.ModifyTakeProfitPips(20);
                }
            }
        }        

        protected override void OnStop()
        {
            // Handle Plugin stop here
        }
    }        
}