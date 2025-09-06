// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    The sample adds a new block into Active Symbol Panel (ASP). The block displays the percentage 
//    difference between the current price of a symbol its price a month ago; the symbol is taken from 
//    the currently active chart frame. This is achieved by handling the ChartManager.ActiveFrameChanged 
//    event.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class ActiveFrameChangedSample : Plugin
    {
        // Declare the necessary UI elements.
        private Grid _grid;
        private TextBlock _percentageTextBlock;
        private Frame _activeFrame;
    
        // This method is triggered when the plugin starts.
        protected override void OnStart()
        {
            // Initialise the grid and the TextBlock displaying the percentage difference.
            _grid = new Grid(1, 1);
            _percentageTextBlock = new TextBlock 
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Monthly change: ",
            };
            
            _grid.AddChild(_percentageTextBlock, 0, 0);  // Add the TextBlock to the grid at row 0, column 0.
            
            // Initialise a new block inside the ASP tab and add the grid as a child.
            var block = Asp.SymbolTab.AddBlock("Monthly Change Plugin");
            
            block.Child = _grid;
            
            // Attach a custom handler to the ActiveFrameChanged event.
            ChartManager.ActiveFrameChanged += ChartManager_ActiveFrameChanged;

        }

        // This method is triggered whenever the active frame changes in the chart manager.
        private void ChartManager_ActiveFrameChanged(ActiveFrameChangedEventArgs obj)
        {
            // Check if the new frame is a ChartFrame.
            if (obj.NewFrame is ChartFrame) 
            {
                // Cast the Frame into a ChartFrame.
                var newChartFrame = obj.NewFrame as ChartFrame;
                
                // Attain market data for the symbol for which the currently active ChartFrame is opened.
                var dailySeries = MarketData.GetBars(TimeFrame.Daily, newChartFrame.Symbol.Name);
                
                // Calculate the monthly change and display it inside the TextBlock.
                double monthlyChange = (newChartFrame.Symbol.Bid - dailySeries.ClosePrices[dailySeries.ClosePrices.Count - 30]) / 100;
                _percentageTextBlock.Text = $"Monthly change: {monthlyChange}";  // Update the TextBlock with the calculated change.
            }
        }


    }        
}
