// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses the AddToChart() method to ensure that all indicators used by the cBot
//    , namely the MACD Crossover and the RSI, are added to the chart on cBot start. The cBot uses
//    both these indicators to guide its trading decisions. Instead of using the AddToChart() method,
//    the sample could have set the AddIndicators property to 'true'.
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
    // If we set AddIndicators to 'true', we do not have to
    // use the AddToChart() method
    // [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    
    [Robot(AccessRights = AccessRights.None)]
    public class AddToChartSample : Robot
    {
        
        MacdCrossOver _macd;
        RelativeStrengthIndex _rsi;

        protected override void OnStart()
        {
            // Initialising our indicators
            _macd = Indicators.MacdCrossOver(26, 19, 9);
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 9);
            
            // Adding both our indicators to the chart
            _rsi.AddToChart();
            _macd.AddToChart();
        }

        protected override void OnBarClosed()
        {
            // Using indicator outputs to execute trading operations
            if (_macd.Histogram.LastValue > 0 && _rsi.Result.LastValue <= 30) 
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000);
            } else if (_macd.Histogram.LastValue < 0 && _rsi.Result.LastValue >= 70) 
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000);
            }
        }
    }
}