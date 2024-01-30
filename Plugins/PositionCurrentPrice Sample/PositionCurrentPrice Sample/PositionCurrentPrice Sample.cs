// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a new block into the ASP and opens a position for EURUSD with a custom label.
//    The text inside the new block shows the current price of EURUSD, which is achieved by using the
//    Position.CurrentPrice property. The text inside the block is updated every 100 milliseconds.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class PositionCurrentPriceSample : Plugin
    {
        private TextBlock _currentPriceText;
        
        
        protected override void OnStart()
        {
            // Configuring the new TextBlock
            _currentPriceText = new TextBlock
            {
                Text = "Initialising...",
                FontSize = 50,
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeight.ExtraBold,
            };
            
            // Adding a new block into the ASP
            Asp.SymbolTab.AddBlock("Position.CurrentPrice").Child = _currentPriceText;
            
            // Opening a new position (the sample assumes that the order
            // is executed successfully)
            ExecuteMarketOrder(TradeType.Buy, "EURUSD", 10000, "plugin position");
            
            // Updating the text inside the TextBlock by using Position.CurrentPrice
            _currentPriceText.Text = Positions.Find("plugin position").CurrentPrice.ToString();
            
            // Starting the timer with 100 milliseconds as the tick
            Timer.Start(TimeSpan.FromMilliseconds(100));
            
        }

        // Overriding the built-in handler of the Timer.TimerTick event
        protected override void OnTimer()
        {
            // Updating the text inside the TextBlock by using Position.CurrentPrice
            _currentPriceText.Text = Positions.Find("plugin position").CurrentPrice.ToString();
        }

    }        
}