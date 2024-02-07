// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample accesses the Chart.Id property of the chart to which it is attached.
//    On chart activation/deactivation, the indicator prints the value of Chart.Id to the log.
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
    public class ChartIdSample : Indicator
    {

        
        string _currentChartId;
        
        // Getting the Id of the current Chart and
        // assigning custom event handlers for the
        // Chart.Activated and Chart.Deactivated events
        protected override void Initialize()
        {
            _currentChartId = Chart.Id.ToString();
            Chart.Activated += Chart_Activated;
            Chart.Deactivated += Chart_Deactivated;
        }


        private void Chart_Deactivated(ChartActivationChangedEventArgs obj)
        {
            Print($"Chart with ID {_currentChartId} deactivated!");
        }

        private void Chart_Activated(ChartActivationChangedEventArgs obj)
        {
            Print($"Chart with ID {_currentChartId} activated!");
        }

        public override void Calculate(int index)
        {

        }
    }
}