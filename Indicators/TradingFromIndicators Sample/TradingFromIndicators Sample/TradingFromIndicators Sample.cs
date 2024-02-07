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
       
        // Defining the custom 'Buy' and 'Sell' buttons
        private Button _buyButton = new Button
        {
            BackgroundColor = Color.LawnGreen,
            Width = 100,
            Height = 50,
            Text = "Buy",
            
        };

        private Button _sellButton = new Button
        {
            BackgroundColor = Color.Orange,
            Width = 100,
            Height = 50,
            Text = "Sell",
        };
        
        // Declaring a new Grid
        private Grid _buttonsGrid = new Grid(2, 1);
        
        protected override void Initialize()
        {
            // Adding the 'Buy' and 'Sell' buttons to the grid
            _buttonsGrid.AddChild(_buyButton, 0, 0);
            _buttonsGrid.AddChild(_sellButton, 1, 0);
            
            // Adding the Grid to the chart 
            Chart.AddControl(_buttonsGrid);
            
            // Assigning custom event handlers for the Click event
            // of both buttons
            _buyButton.Click += OnBuyButtonClick;
            _sellButton.Click += OnSellButtonClick;
        }
        
        // Executing a market order on each click on the 'Buy' button
        private void OnSellButtonClick(ButtonClickEventArgs obj)
        {
            ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000);
        }

        // Executing a market order on each click of the 'Sell' button
        private void OnBuyButtonClick(ButtonClickEventArgs obj)
        {
            ExecuteMarketOrder(TradeType.Sell, SymbolName, 10000);
        }

        public override void Calculate(int index)
        {

        }
        
    }
}