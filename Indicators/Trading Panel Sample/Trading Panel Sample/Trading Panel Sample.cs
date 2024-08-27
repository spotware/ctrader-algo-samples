// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This sample indicator displays a simple trading panel.   
//
//    For a detailed tutorial on creating this indicator, see this video: https://www.youtube.com/watch?v=IJu7zxl5DA0
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class TradingPanel : Indicator
    {
        protected override void Initialize()
        {
            var tradeButtonBuy = new Button
            {
                Text = "Buy",
                ForegroundColor = Color.White,
                BackgroundColor = Color.Green,
                Height = 25,
                Width = 75,
                Margin = 2
            };

            tradeButtonBuy.Click += args => ExecuteMarketOrderAsync(TradeType.Buy, SymbolName, 1000);

            var tradeButtonSell = new Button
            {
                Text = "Sell",
                ForegroundColor = Color.White,
                BackgroundColor = Color.Red,
                Height = 25,
                Width = 75,
                Margin = 2
            };
            tradeButtonSell.Click += args => ExecuteMarketOrderAsync(TradeType.Sell, SymbolName, 1000);

            var grid = new Grid(1, 2);
            grid.AddChild(tradeButtonBuy, 0,0);
            grid.AddChild(tradeButtonSell, 0, 1);
            Chart.AddControl(grid);
        }

        public override void Calculate(int index)
        {
            // Calculate value at specified index
            // Result[index] = 
        }
    }
}