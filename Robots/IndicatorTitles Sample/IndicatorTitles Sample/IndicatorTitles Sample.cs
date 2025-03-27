// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses the Chart.DisplaySettings.IndicatorTitles property
//    and prints its value on initialisation and whenever the user changes
//    the chart display options.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as AccessRights and its ability to add indicators.  
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class IndicatorTitlesSample : Robot
    {
        // This method is called when the cBot is initialised.
        protected override void OnStart()
        {
            // Adding a custom event handler for the DisplaySettingsChanged event
            // This event is triggered when the user changes the chart display settings.
            Chart.DisplaySettingsChanged += OnDisplaySettingsChanged;

            // Print the initial value of IndicatorTitles when the cBot starts.
            Print(Chart.DisplaySettings.IndicatorTitles);
        }

        // This method is called whenever the chart's display settings are changed.
        protected void OnDisplaySettingsChanged(ChartDisplaySettingsEventArgs args)
        {
            // Print the updated value of IndicatorTitles whenever the display settings change.
            Print(Chart.DisplaySettings.IndicatorTitles);
        }
    }
}
