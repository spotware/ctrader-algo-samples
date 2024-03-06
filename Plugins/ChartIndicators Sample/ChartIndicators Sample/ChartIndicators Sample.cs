// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a new block into the Asp. The block contains a single button. Upon the user
//    pressing the button, the plugin uses the Chart.Indicators.Remove() method to delete all indicators
//    attached to the currently active chart.
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
    public class ChartIndicatorsSample : Plugin
    {
        
        // Declaring the button to be placed inside
        // the custom ASP block
        private Button _removeIndicatorsButton;
        
        protected override void OnStart()
        {
            // Initialising the button
            _removeIndicatorsButton = new Button
            {
                BackgroundColor = Color.Orange,
                Text = "Remove All Indicators",
            };
            
            // Handling the Click event for the button
            _removeIndicatorsButton.Click += RemoveIndicatorsButtonOnClick;
            
            // Adding a new block into the ASP and setting its child
            Asp.SymbolTab.AddBlock("Indicators Removal").Child = _removeIndicatorsButton;
            
            
        }

        protected void RemoveIndicatorsButtonOnClick(ButtonClickEventArgs args)
        {
            // Attaining the currently active ChartFrame
            var activeChartFrame = ChartManager.ChartContainers.MainChartContainer.ActiveFrame as ChartFrame;
            
            // Attaining the Chart from the ChartFrame
            var activeChart = activeChartFrame.Chart;
            
            // Iterating over all indicators attached to the Chart
            // and deleting them
            foreach (var indicator in activeChart.Indicators)
            {
                activeChart.Indicators.Remove(indicator);
            }
        }
    }        

}        
