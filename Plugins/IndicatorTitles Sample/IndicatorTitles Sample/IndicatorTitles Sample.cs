// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample uses the Chart.DisplaySettings.IndicatorTitles property
//    and prints its value on initialisation and whenever the user changes
//    the chart display options. This is done by first opening a new chart
//    for EURUSD and then assigning a custom event handler for the DisplaySettingsChanged
//    event.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    // Declaring the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class IndicatorTitlesSample : Plugin
    {
        // Declaring a new chart variable.
        Chart _newChart;
        
        // This method is executed when the plugin starts.
        protected override void OnStart()
        {
            // Creating a new chart for EURUSD on plugin start.
            _newChart = ChartManager.AddChartFrame("EURUSD", TimeFrame.Daily).Chart;
            
            // Adding a custom event handler for the DisplaySettingsChanged event.
            _newChart.DisplaySettingsChanged += OnDisplaySettingsChanged;

            Print(_newChart.DisplaySettings.IndicatorTitles);
        }
        
        // Defining the custom event handler for the DisplaySettingsChanged event.
        protected void OnDisplaySettingsChanged(ChartDisplaySettingsEventArgs args)
        {
            Print(_newChart.DisplaySettings.IndicatorTitles);
        }
    }        
}
