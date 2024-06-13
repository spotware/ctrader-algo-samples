// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This example plugin adds a new section to the Active Symbol Panel (ASP), with styles applied to the plugin controls.
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
    [Plugin(AccessRights = AccessRights.None)]
    public class MyASPExample : Plugin
    {
    
        TextBlock _txtBuyVWAP;
        TextBlock _txtSellVWAP;
        protected override void OnStart()
        {
            var block = Asp.SymbolTab.AddBlock("My ASP Example");
            block.Index = 2;
            block.Height = 100;
            block.IsExpanded = true;
            
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            
            var textBoxStyle = new Style();
            textBoxStyle.Set(ControlProperty.Width, 200);
            textBoxStyle.Set(ControlProperty.Margin, 5);
            textBoxStyle.Set(ControlProperty.FontFamily, "Cambria");
            textBoxStyle.Set(ControlProperty.FontSize, 15);
            
            _txtBuyVWAP = new TextBlock
            {
                Text = "Buy Text Block",
                Style = textBoxStyle,
                ForegroundColor = Color.Green
            };
            
            _txtSellVWAP = new TextBlock
            {
                Text = "Sell Text Block",
                Style = textBoxStyle,
                ForegroundColor = Color.Red
            };
            
            panel.AddChild(_txtBuyVWAP);
            panel.AddChild(_txtSellVWAP);
            
            block.Child = panel;
            
            var buyPositions = Positions.Where(p => p.TradeType == TradeType.Buy);
            _txtBuyVWAP.Text = "Buy Positions VWAP: " + Math.Round((buyPositions.Sum(p => p.EntryPrice * p.VolumeInUnits)/ buyPositions.Sum(p => p.VolumeInUnits)), 5);
            
            var sellPositions = Positions.Where(p => p.TradeType == TradeType.Sell);
            _txtSellVWAP.Text = "Sell Positions VWAP: " + Math.Round((sellPositions.Sum(p => p.EntryPrice * p.VolumeInUnits)/ sellPositions.Sum(p => p.VolumeInUnits)), 5);
        }

        protected override void OnStop()
        {
            // Handle Plugin stop here
        }
    }        
}