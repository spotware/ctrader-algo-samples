// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses AlgoRegistry to determine whether the user has installed two custom fictional
//    indicators, namely Rapid SMA and Swirling SMA. If the indicators are not installed, the indicator
//    displays a message box alerting the user to this fact.
//
// -------------------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class AlgoRegistrySample : Indicator
    {

        protected override void Initialize()
        {
            // Checking if the user has installed the required indicators
            if (!AlgoRegistry.Exists("Rapid SMA") && !AlgoRegistry.Exists("Swirling SMA"))
            {
                // Showing the message box with the warning
                MessageBox.Show("Required indicators are not installed!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
            }
            else
            {
                // Adding the indicators to the chart if they are installed
                ChartIndicators.Add("Rapid SMA");
                ChartIndicators.Add("Swirling SMA");
            }
        }

        public override void Calculate(int index)
        {
            // Result[index] = 
        }
    }
}