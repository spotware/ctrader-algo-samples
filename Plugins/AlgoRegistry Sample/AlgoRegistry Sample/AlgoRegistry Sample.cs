// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a new block into the ASP. The block displays statistics about the number of
//    different types of algorithms installed on the user's machine. Information in the block is
//    updated every second to make sure that it is always accurate.
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

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class AlgoRegistrySample : Plugin
    {
        // Declaring the TextBlock for displaying data about
        // custom indicators
        private TextBlock _customIndicatorsBlock = new TextBlock
        {
            FontSize = 12,
            FontWeight = FontWeight.Bold,
            TextAlignment = TextAlignment.Left,
            Padding = new Thickness(5, 5, 5, 5),
        };

        // Declaring the TextBlock for displaying data about
        // built-in indicators
        private TextBlock _indicatorsBlock = new TextBlock
        {
            FontSize = 12,
            FontWeight = FontWeight.Bold,
            TextAlignment = TextAlignment.Left,
            Padding = new Thickness(5, 5, 5, 5),
        };

        // Declaring the TextBlock for displaying data about
        // cBots
        private TextBlock _robotsBlock = new TextBlock
        {
            FontSize = 12,
            FontWeight = FontWeight.Bold,
            TextAlignment = TextAlignment.Left,
            Padding = new Thickness(5, 5, 5, 5),
        };

        // Declaring the TextBlock for displaying data about
        // plugins
        private TextBlock _pluginsBlock = new TextBlock
        {
            FontSize = 12,
            FontWeight = FontWeight.Bold,
            TextAlignment = TextAlignment.Left,
            Padding = new Thickness(5, 5, 5, 5),
        };

        // Declaring the Grid to store custom controls
        private Grid _grid = new Grid(4, 1);
        
        protected override void OnStart()
        {
            // Adding a new block into the ASP and adding
            // the Grid with all TextBlocks as a child
            var aspBlock = Asp.SymbolTab.AddBlock("Algo Registry");
            aspBlock.IsExpanded = true;
            aspBlock.Height = 200;
            
            _grid.AddChild(_robotsBlock, 0, 0);
            _grid.AddChild(_indicatorsBlock, 1, 0);
            _grid.AddChild(_customIndicatorsBlock, 2, 0);
            _grid.AddChild(_pluginsBlock, 3, 0);

            aspBlock.Child = _grid;

            // Starting the Timer to refresh information
            // in all TextBlocks
            Timer.Start(TimeSpan.FromSeconds(1));
        }
        
        protected override void OnTimer() 
        {
            // Using the AlgoRegistry to attain information about the
            // different AlgoKinds installed on the user's machine
            _robotsBlock.Text = $"cBots: {AlgoRegistry.GetCount(AlgoKind.Robot)}";
            _customIndicatorsBlock.Text = $"Custom indicators: {AlgoRegistry.GetCount(AlgoKind.CustomIndicator)}";
            _indicatorsBlock.Text = $"Indicators: {AlgoRegistry.GetCount(AlgoKind.StandardIndicator)}";
            _pluginsBlock.Text = $"Plugins: {AlgoRegistry.GetCount(AlgoKind.Plugin)}";
        }

    }
}