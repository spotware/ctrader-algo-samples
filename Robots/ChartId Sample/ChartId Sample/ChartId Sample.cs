// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample accesses the Chart.Id property of the chart to which it is attached.
//    When the BarClosed event is triggered, the cBot places a new market order and prints data
//    to the log.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System.IO;


namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class ChartIdSample : Robot
    {
        private List<string> _operationsCache = new List<string>();
        
        protected override void OnBarClosed()
        {
            ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000);
            Print($"{DateTime.Now.ToShortTimeString()}: Order Placed on Chart {Chart.Id}");
        }

    }
}