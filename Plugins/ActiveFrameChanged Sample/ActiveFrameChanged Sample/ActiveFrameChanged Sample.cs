// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a new block into the ASP. The block displays the percentage difference between
//    the current price of a symbol its price a month ago; the symbol is taken from the currently active
//    chart frame. This is achieved by handling the ChartManager.ActiveFrameChanged event.
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
    public class ActiveFrameChangedSample : Plugin
    {
    
        // Declaring the necessary UI elements
        private Grid _grid;
        private TextBlock _percentageTextBlock;
        private Frame _activeFrame;
    
        protected override void OnStart()
        {
            // Initialising the grid and the TextBlock
            // displaying the percentage difference
            _grid = new Grid(1, 1);
            _percentageTextBlock = new TextBlock 
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Monthly change: ",
            };
            
            _grid.AddChild(_percentageTextBlock, 0, 0);
            
            // Initialising a new block inside the ASP
            // and adding the grid as a child
            var block = Asp.SymbolTab.AddBlock("Monthly Change Plugin");
            
            block.Child = _grid;
            
            // Attaching a custom handler to the
            // ActiveFrameChanged event
            ChartManager.ActiveFrameChanged += ChartManager_ActiveFrameChanged;

        }

        private void ChartManager_ActiveFrameChanged(ActiveFrameChangedEventArgs obj)
        {
            if (obj.NewFrame is ChartFrame) 
            {
                // Casting the Frame into a ChartFrame
                var newChartFrame = obj.NewFrame as ChartFrame;
                
                // Attaining market data for the symbol for which
                // the currently active ChartFrame is opened
                var dailySeries = MarketData.GetBars(TimeFrame.Daily, newChartFrame.Symbol.Name);
                
                // Calculating the monthly change and displaying it
                // inside the TextBlock
                double monthlyChange = (newChartFrame.Symbol.Bid - dailySeries.ClosePrices[dailySeries.ClosePrices.Count - 30]) / 100;
                _percentageTextBlock.Text = $"Monthly change: {monthlyChange}";
            }
        }


    }        
}