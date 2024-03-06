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
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class IndicatorTitlesSample : Robot
    {
        protected override void OnStart()
        {
            // Adding a custom event handler for the DisplaySettingsChanged event
            Chart.DisplaySettingsChanged += OnDisplaySettingsChanged;

            Print(Chart.DisplaySettings.IndicatorTitles);
        }

        protected void OnDisplaySettingsChanged(ChartDisplaySettingsEventArgs args)
        {
            Print(Chart.DisplaySettings.IndicatorTitles);
        }
    }
}