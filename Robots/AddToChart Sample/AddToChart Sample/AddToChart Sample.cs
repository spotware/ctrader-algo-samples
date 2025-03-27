// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This sample uses the AddToChart method to ensure that all indicators used by the cBot,
//    namely the MACD Crossover and the RSI, are added to the chart on cBot start. The cBot uses
//    both these indicators to guide its trading decisions. Instead of using the AddToChart method,
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
    // Define the cBot attributes.
    // If we set AddIndicators to 'true', we do not have to use the AddToChart() method.
    // [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    [Robot(AccessRights = AccessRights.None)]
    
    public class AddToChartSample : Robot
    {

        // Fields for the MACD and RSI indicators used in the cBot trading logic.
        MacdCrossOver _macd;
        RelativeStrengthIndex _rsi;

        // This method is called when the cBot starts and is used for initialisation.        
        protected override void OnStart()
        {
            // Initialise the MACD indicator with parameters for the fast, slow and signal line periods.
            _macd = Indicators.MacdCrossOver(26, 19, 9);

            // Initialise the RSI indicator with a period of 9, using close prices as the data source.           
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 9);
            
            // Add both indicators to the chart.
            _rsi.AddToChart();
            _macd.AddToChart();
        }

        // This method is called every time a bar closes and is used for trade execution logic.       
        protected override void OnBarClosed()
        {
            // Trading logic based on indicator values.

            // If MACD histogram is positive and RSI is below or equal to 30, open a buy order.            
            if (_macd.Histogram.LastValue > 0 && _rsi.Result.LastValue <= 30) 
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000);  // Open a market order to buy with a volume of 10,000 units.
            } 

            // If MACD histogram is negative and RSI is above or equal to 70, open a sell order.
            else if (_macd.Histogram.LastValue < 0 && _rsi.Result.LastValue >= 70) 
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000);  // Open a market order to sell order with a volume of 10,000 units.
            }
        }
    }
}
