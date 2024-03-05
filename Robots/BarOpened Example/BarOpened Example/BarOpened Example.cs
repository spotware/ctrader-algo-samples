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
    public class BarOpenedExample : Robot
    {
        protected override void OnBar() 
        {
            var previousBar = Bars[Bars.Count - 2];
            var priceDifference = ((Bars.LastBar.Open - previousBar.Open) / previousBar.Open) * 100;
            
            if (priceDifference > 1) 
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000);
            }
            else if (priceDifference < -1) 
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000);
            }
            else 
            {
                foreach (var position in Positions) 
                {
                    position.Close();
                }
            }
        }
    }
}