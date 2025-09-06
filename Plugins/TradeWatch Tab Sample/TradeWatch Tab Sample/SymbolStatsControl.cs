// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//  
//    The sample creates a custom control that displays the real-time stats of a selected trading symbol,
//    such as the symbol bid, ask, spread, unrealized profits and commission. The control dynamically 
//    updates the displayed information whenever the symbol ticks.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Plugins;

public class SymbolStatsControl : CustomControl  // Declare the custom control class to display symbol stats.
{
    private const string FontFamily = "Calibri";  // Define a constant for font family.
    private readonly TextBlock _commissionTextBlock;  // Declare a TextBlock for displaying commission.

    private readonly StackPanel _panel;  // Declare a StackPanel to hold UI controls vertically.
    private readonly TextBlock _spreadTextBlock;  // Declare a TextBlock to display the spread.
    private readonly TextBlock _symbolNameTextBlock;  // Declare a TextBlock to display the symbol name.
    private readonly TextBlock _symbolPriceTextBlock;  // Declare a TextBlock to display the symbol price.

    private readonly TextBlock _unrealizedGrossProfitTextBlock;  // Declare a TextBlock to show unrealised gross profit.
    private readonly TextBlock _unrealizedNetProfitTextBlock;  // Declare a TextBlock to show unrealised net profit.

    private Symbol _symbol;  // Declare a private field to store the symbol.

    // This method is used to set up the custom control.
    public SymbolStatsControl()
    {
        _panel = new StackPanel  // Create a StackPanel to organise controls vertically.
        {
            Orientation = Orientation.Vertical,  // Set the orientation to vertical.
        };

        _symbolNameTextBlock = GetTextBlock();  // Initialise the symbol name TextBlock.
        _symbolPriceTextBlock = GetTextBlock();  // Initialise the symbol price TextBlock.

        _panel.AddChild(WrapInHorizontalPanel(_symbolNameTextBlock, _symbolPriceTextBlock));  // Wrap and add the name and price text blocks in a horizontal panel.

        var grid = new Grid(10, 2);  // Create a 10x2 grid to organise the remaining stats.

        _spreadTextBlock = GetTextBlock();  // Initialise the spread TextBlock.

        grid.AddChild(GetTextBlock("Spread"), 0, 0);  // Add a label for spread to the grid.
        grid.AddChild(_spreadTextBlock, 0, 1);  // Add the spread value TextBlock to the grid.

        _unrealizedNetProfitTextBlock = GetTextBlock();  // Initialise the unrealised net profit TextBlock.

        grid.AddChild(GetTextBlock("Unrealized Net Profit"), 1, 0);  // Add a label for unrealised net profit to the grid.
        grid.AddChild(_unrealizedNetProfitTextBlock, 1, 1);  // Add the unrealised net profit TextBlock to the grid.

        _unrealizedGrossProfitTextBlock = GetTextBlock();  // Initialise the unrealised gross profit TextBlock.

        grid.AddChild(GetTextBlock("Unrealized Gross Profit"), 2, 0);  // Add a label for unrealised gross profit to the grid.
        grid.AddChild(_unrealizedGrossProfitTextBlock, 2, 1);  // Add the unrealised gross profit TextBlock to the grid.

        _commissionTextBlock = GetTextBlock();  // Initialise the commission TextBlock.

        grid.AddChild(GetTextBlock("Commission"), 3, 0);  // Add a label for commission to the grid.
        grid.AddChild(_commissionTextBlock, 3, 1);  // Add the commission TextBlock to the grid.

        _panel.AddChild(grid);  // Add the grid to the panel.

        AddChild(_panel);  // Add the entire panel to the custom control.
    }

    // Property to get or set the symbol for this control.
    public Symbol Symbol
    {
        get => _symbol;  // Return the current symbol.
        set
        {
            if (value is null || _symbol == value)  // Check if the symbol is null or has not changed.
                return;  // If no change, do nothing.

            Update(value);  // If the symbol has changed, update the control.
        }
    }

    // Helper method to create a horizontal panel.
    private StackPanel WrapInHorizontalPanel(params ControlBase[] controls)
    {
        var panel = new StackPanel  // Create a new StackPanel for horizontal layout.
        {
            Orientation = Orientation.Horizontal  // Set orientation to horizontal.
        };

        foreach (var control in controls)  // Iterate over each control passed.
        {
            if (control.Margin == 0)  // Check if the control has no margin.
                control.Margin = 3;  // Set a default margin if no margin is specified.

            panel.AddChild(control);  // Add the control to the panel.
        }

        return panel;  // Return the constructed horizontal panel.
    }

    // Method to update the stats for the new symbol.
    private void Update(Symbol newSymbol)
    {
        var previousSymbol = _symbol;  // Store the current symbol to unsubscribe from the tick event.
        _symbol = newSymbol;  // Update the symbol to the new one.

        // Update the text values of the TextBlocks to reflect the new symbol data.
        _symbolNameTextBlock.Text = $"{newSymbol.Name} ({newSymbol.Description})";
        _symbolPriceTextBlock.Text = $"Bid: {newSymbol.Bid}, Ask: {newSymbol.Ask}";
        _spreadTextBlock.Text = $"{GetSpreadInPips(newSymbol)} Pips";
        _unrealizedNetProfitTextBlock.Text = newSymbol.UnrealizedNetProfit.ToString();
        _unrealizedGrossProfitTextBlock.Text = newSymbol.UnrealizedGrossProfit.ToString();
        _commissionTextBlock.Text = $"{newSymbol.Commission} {newSymbol.CommissionType}";

        if (previousSymbol is not null) previousSymbol.Tick -= OnSymbolTick;  // Unsubscribe from the previous tick event.

        newSymbol.Tick += OnSymbolTick;  // Subscribe to the new tick event.
    }

    // Event handler for symbol tick events.
    private void OnSymbolTick(SymbolTickEventArgs obj)
    {
        // Update the displayed symbol data when the symbol ticks.
        _symbolPriceTextBlock.Text = $"Bid: {obj.Bid}, Ask: {obj.Ask}";
        _spreadTextBlock.Text = $"{GetSpreadInPips(obj.Symbol)} Pips";
        _unrealizedNetProfitTextBlock.Text = obj.Symbol.UnrealizedNetProfit.ToString();
        _unrealizedGrossProfitTextBlock.Text = obj.Symbol.UnrealizedGrossProfit.ToString();
    }

    // Method to calculate the spread in pips.
    private double GetSpreadInPips(Symbol symbol) =>
        
        // Calculate the spread in pips based on the spread, digits, pip size and tick size.
        (symbol.Spread * Math.Pow(10, symbol.Digits)) / (Symbol.PipSize / Symbol.TickSize);

    // Helper method to create a TextBlock with common properties.
    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 20,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}
