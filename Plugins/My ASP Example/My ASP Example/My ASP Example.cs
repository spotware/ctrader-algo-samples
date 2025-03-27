// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    The sample adds a new section to Active Symbol Panel (ASP), with styles applied to the plugin controls.
//
//    For a detailed tutorial on creating this plugin, watch the video at: https://www.youtube.com/watch?v=WRwhT7jxTNs
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
  // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class MyASPExample : Plugin
    {
        TextBlock _txtBuyVWAP;  // TextBlock to display the Buy VWAP (Volume Weighted Average Price).
        TextBlock _txtSellVWAP;  // TextBlock to display the Sell VWAP.
        
        // This method is executed when the plugin starts.
        protected override void OnStart()
        {
            // Create a new block in the ASP.
            var block = Asp.SymbolTab.AddBlock("My ASP Example");
            block.Index = 2;
            block.Height = 100;
            block.IsExpanded = true;
            
            // Create a vertical stack panel to arrange child controls.
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            
            // Create a style for the text blocks (used for both Buy and Sell VWAP).
            var textBoxStyle = new Style();
            textBoxStyle.Set(ControlProperty.Width, 200);
            textBoxStyle.Set(ControlProperty.Margin, 5);
            textBoxStyle.Set(ControlProperty.FontFamily, "Cambria");
            textBoxStyle.Set(ControlProperty.FontSize, 15);
            
            // Create a text block for displaying the Buy VWAP.
            _txtBuyVWAP = new TextBlock
            {
                Text = "Buy Text Block",
                Style = textBoxStyle,
                ForegroundColor = Color.Green
            };
            
            // Create a text block for displaying the Sell VWAP.
            _txtSellVWAP = new TextBlock
            {
                Text = "Sell Text Block",
                Style = textBoxStyle,
                ForegroundColor = Color.Red
            };
            
            // Add these text blocks to the panel.
            panel.AddChild(_txtBuyVWAP);
            panel.AddChild(_txtSellVWAP);
            
            // Assign the panel to the block child, which displays the content.
            block.Child = panel;
            
            // Calculate and display the Buy VWAP.
            var buyPositions = Positions.Where(p => p.TradeType == TradeType.Buy);  // Get all buy positions.
            // Calculate VWAP for buy positions.
            _txtBuyVWAP.Text = "Buy Positions VWAP: " + Math.Round((buyPositions.Sum(p => p.EntryPrice * p.VolumeInUnits)/ buyPositions.Sum(p => p.VolumeInUnits)), 5);
            
            // Calculate and display the Sell VWAP.
            var sellPositions = Positions.Where(p => p.TradeType == TradeType.Sell);  // Get all sell positions.
            // Calculate VWAP for sell positions.
            _txtSellVWAP.Text = "Sell Positions VWAP: " + Math.Round((sellPositions.Sum(p => p.EntryPrice * p.VolumeInUnits)/ sellPositions.Sum(p => p.VolumeInUnits)), 5);
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here.
        }
    }        
}
