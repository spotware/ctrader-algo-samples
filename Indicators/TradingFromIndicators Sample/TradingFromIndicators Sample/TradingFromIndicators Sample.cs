// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The indicator places and cancels pending orders depending on SMA crossovers. To monitor crossovers,
//    the sample uses the built-in Timer, and the HasCrossedAbove() and HasCrossedBelow() methods.
//    When the first pending order is placed, the indicator displays a message box with a warning
//    prompting the user to give permissions for trading.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class TradingFromIndicatorsSample : Indicator
    {
        // Declaring two SMAs
        SimpleMovingAverage _slowSMA;
        SimpleMovingAverage _fastSMA;
        
        protected override void Initialize()
        {
            // Initialising two SMAs
            _slowSMA = Indicators.SimpleMovingAverage(Bars.ClosePrices, 50);
            _fastSMA = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
            
            // Starting the timer on indicator initialisation
            Timer.Start(TimeSpan.FromMinutes(1));
        }

        public override void Calculate(int index)
        {

        }
        
        // Overriding the built-in handler of the Timer.Tick event
        protected override void OnTimer() 
        {
            // Checking if the 'fast' SMA has crossed above the 'slow' SMA
            if (_fastSMA.Result.HasCrossedAbove(_slowSMA.Result, 20)) 
            {
            
                // Cancelling the last pending order placed
                PendingOrders[-1].Cancel();
                
                // Placing a new limit order above the current bid price
                PlaceLimitOrder(TradeType.Sell, SymbolName, 10000, Symbol.Bid * 1.05);
            
            // Checking if the 'fast' SMA has crossed below the 'slow' SMA
            } else if (_fastSMA.Result.HasCrossedBelow(_slowSMA.Result, 20)) 
            {
                // Cancelling the last pending order placed
                PendingOrders[-1].Cancel();
                
                // Placing a new limit order below the current bid price
                PlaceLimitOrder(TradeType.Buy, SymbolName, 10000, Symbol.Bid * 0.95);
            }
        }
    }
}