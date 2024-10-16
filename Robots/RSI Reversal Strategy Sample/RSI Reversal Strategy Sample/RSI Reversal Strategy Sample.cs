// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example cBot implements a strategy based on the Relative Strength Index (RSI) indicator reversal. 
//
//    For a detailed tutorial on creating this cBot, watch the video at: https://youtu.be/mEoIvP11Z1U
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class RSIReversalStrategySample : Robot
    {
        [Parameter(DefaultValue = 30)]
        public int BuyLevel { get; set; }

        [Parameter(DefaultValue = 70)]
        public int SellLevel { get; set; }

        private RelativeStrengthIndex _rsi;

        protected override void OnStart()
        {
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 14);
        }

        protected override void OnBarClosed()
        {
            if (_rsi.Result.LastValue < BuyLevel)
            {
                if (Positions.Count == 0)
                    ExecuteMarketOrder(TradeType.Buy, SymbolName, 1000);
                foreach (var position in Positions.Where(p => p.TradeType == TradeType.Sell))
                {
                    position.Close();
                }

            }
            
            else if (_rsi.Result.LastValue > SellLevel)
            {
                if (Positions.Count == 0)
                    ExecuteMarketOrder(TradeType.Sell, SymbolName, 1000);
                foreach (var position in Positions.Where(p => p.TradeType == TradeType.Buy))
                {
                    position.Close();
                }
            }
        }

        protected override void OnStop()
        {

        }
    }
}