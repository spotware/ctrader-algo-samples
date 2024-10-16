// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example cBot trades the hammer pattern for long entries and the hanging man pattern for short entries.
//
//    For a detailed tutorial on creating this cBot, watch the video at: https://youtu.be/mEoIvP11Z1U
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
    public class PatternsStrategySample : Robot
    {
        [Parameter(DefaultValue = 1000)]
        public double Volume { get; set; }

        [Parameter(DefaultValue = 10)]
        public double StopLoss { get; set; }

        [Parameter(DefaultValue = 10)]
        public double TakeProfit { get; set; }

        protected override void OnStart()
        {

        }

        protected override void OnBarClosed()
        {
            if (Bars.Last(0).Close == Bars.Last(0).High &&
               (Bars.Last(0).Close - Bars.Last(0).Open) < (Bars.Last(0).Close - Bars.Last(0).Low) * 0.2)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);
            }

            if (Bars.Last(0).Close == Bars.Last(0).Low &&
               (Bars.Last(0).Open - Bars.Last(0).Close) < (Bars.Last(0).High - Bars.Last(0).Close) * 0.2)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, Volume, InstanceId, StopLoss, TakeProfit);
            }

        }

        protected override void OnStop()
        {

        }
    }
}