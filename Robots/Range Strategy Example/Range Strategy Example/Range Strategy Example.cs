// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
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
    public class RangeStrategyExample : Robot
    {
        [Parameter(DefaultValue = 100000)]
        public double Volume { get; set; }

        [Parameter(DefaultValue = 20)]
        public double StopLoss { get; set; }

        [Parameter(DefaultValue = 20)]
        public double TakeProfit { get; set; }

        protected override void OnStart()
        {

        }

        protected override void OnBarClosed()
        {
            if (Bars.Last(0).Close > Bars.Last(0).Open && Bars.Last(1).Close < Bars.Last(1).Open &&
            Bars.Last(0).Close > Bars.Last(1).Open)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);
            }


            if (Bars.Last(0).Close < Bars.Last(0).Open && Bars.Last(1).Close > Bars.Last(1).Open &&
            Bars.Last(0).Close < Bars.Last(1).Open)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);
            }
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}