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
    public class BarClosedExample : Robot
    {
        protected override void OnBarClosed() 
        {
            var lowCloseDifference = ((Bars.LastBar.Close - Bars.LastBar.Low) / Bars.LastBar.Close) * 100;
            if (lowCloseDifference > 0.5) 
            {
                foreach (var position in Positions) 
                {
                    position.Close();
                }
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000, null, null, 50);
            }
        }
    }
}