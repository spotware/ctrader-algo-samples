using System;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Plugins;

public class SymbolStatsControl : CustomControl
{
    private const string FontFamily = "Calibri";
    private readonly TextBlock _commissionTextBlock;

    private readonly StackPanel _panel;
    private readonly TextBlock _spreadTextBlock;
    private readonly TextBlock _symbolNameTextBlock;
    private readonly TextBlock _symbolPriceTextBlock;

    private readonly TextBlock _unrealizedGrossProfitTextBlock;
    private readonly TextBlock _unrealizedNetProfitTextBlock;

    private Symbol _symbol;

    public SymbolStatsControl()
    {
        _panel = new StackPanel
        {
            Orientation = Orientation.Vertical,
        };

        _symbolNameTextBlock = GetTextBlock();
        _symbolPriceTextBlock = GetTextBlock();

        _panel.AddChild(WrapInHorizontalPanel(_symbolNameTextBlock, _symbolPriceTextBlock));

        var grid = new Grid(10, 2);

        _spreadTextBlock = GetTextBlock();

        grid.AddChild(GetTextBlock("Spread"), 0, 0);
        grid.AddChild(_spreadTextBlock, 0, 1);

        _unrealizedNetProfitTextBlock = GetTextBlock();

        grid.AddChild(GetTextBlock("Unrealized Net Profit"), 1, 0);
        grid.AddChild(_unrealizedNetProfitTextBlock, 1, 1);

        _unrealizedGrossProfitTextBlock = GetTextBlock();

        grid.AddChild(GetTextBlock("Unrealized Gross Profit"), 2, 0);
        grid.AddChild(_unrealizedGrossProfitTextBlock, 2, 1);

        _commissionTextBlock = GetTextBlock();

        grid.AddChild(GetTextBlock("Commission"), 3, 0);
        grid.AddChild(_commissionTextBlock, 3, 1);

        _panel.AddChild(grid);

        AddChild(_panel);
    }

    public Symbol Symbol
    {
        get => _symbol;
        set
        {
            if (value is null || _symbol == value)
                return;

            Update(value);
        }
    }

    private StackPanel WrapInHorizontalPanel(params ControlBase[] controls)
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };

        foreach (var control in controls)
        {
            if (control.Margin == 0)
                control.Margin = 3;

            panel.AddChild(control);
        }

        return panel;
    }

    private void Update(Symbol newSymbol)
    {
        var previousSymbol = _symbol;
        _symbol = newSymbol;

        _symbolNameTextBlock.Text = $"{newSymbol.Name} ({newSymbol.Description})";
        _symbolPriceTextBlock.Text = $"Bid: {newSymbol.Bid}, Ask: {newSymbol.Ask}";
        _spreadTextBlock.Text = $"{GetSpreadInPips(newSymbol)} Pips";
        _unrealizedNetProfitTextBlock.Text = newSymbol.UnrealizedNetProfit.ToString();
        _unrealizedGrossProfitTextBlock.Text = newSymbol.UnrealizedGrossProfit.ToString();
        _commissionTextBlock.Text = $"{newSymbol.Commission} {newSymbol.CommissionType}";

        if (previousSymbol is not null) previousSymbol.Tick -= OnSymbolTick;

        newSymbol.Tick += OnSymbolTick;
    }

    private void OnSymbolTick(SymbolTickEventArgs obj)
    {
        _symbolPriceTextBlock.Text = $"Bid: {obj.Bid}, Ask: {obj.Ask}";
        _spreadTextBlock.Text = $"{GetSpreadInPips(obj.Symbol)} Pips";
        _unrealizedNetProfitTextBlock.Text = obj.Symbol.UnrealizedNetProfit.ToString();
        _unrealizedGrossProfitTextBlock.Text = obj.Symbol.UnrealizedGrossProfit.ToString();
    }

    private double GetSpreadInPips(Symbol symbol) =>
        (symbol.Spread * Math.Pow(10, symbol.Digits)) / (Symbol.PipSize / Symbol.TickSize);

    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 20,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}