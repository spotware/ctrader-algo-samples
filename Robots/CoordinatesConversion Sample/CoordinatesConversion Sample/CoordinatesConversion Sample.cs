// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses a custom event handler for the Chart.MouseUp event to place orders whenever
//    a user right-clicks on the chart to which the cBot is attached. To achieve this, the sample
//    uses the Chart.YToYValue() method, which convers the Y-coordinate of the mouse cursor to
//    a valid symbol price at which an order is placed.
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

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None)]
    public class CoordinatesConverter : Robot
    {
        protected override void OnStart()
        {
            // Assigning a custom event handler to the Chart.MouseUp event
            Chart.MouseUp += Chart_MouseUp;
        }

        private void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            // Using the Chart.YToYValue() method to convert coordinates
            // into a symbol price
            var desiredPrice = Chart.YToYValue(obj.MouseY);
            
            // Using a ternary operator to determine the trade type
            // A buy order is placed if the mouse is under the current symbol bid
            // A sell order is placed if the mouse is above the current symbol bid
            var desiredTradeType = desiredPrice > Symbol.Bid ? TradeType.Sell : TradeType.Buy;
            
            PlaceLimitOrder(desiredTradeType, SymbolName, 10000, desiredPrice);
        }
    }
}