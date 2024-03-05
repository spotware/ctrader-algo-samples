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
    public class CustomHandlersExample : Robot
    {

        protected override void OnStart()
        {
            Bars.BarOpened += BullishReversal;
            Bars.BarOpened += BearishReversal;
        }

        
        private void BullishReversal(BarOpenedEventArgs args) 
        {
            if (Bars.LastBar.Open > Bars.Last(1).Close && Bars.LastBar.Open > Bars.Last(2).Close) 
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000, null, 10, 50);
            }
        }
        
        private void BearishReversal(BarOpenedEventArgs args) 
        {
            if (Bars.LastBar.Open < Bars.Last(1).Close && Bars.LastBar.Open < Bars.Last(2).Close) 
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000, null, 10, 50);
            }
        }
                
    }
}

