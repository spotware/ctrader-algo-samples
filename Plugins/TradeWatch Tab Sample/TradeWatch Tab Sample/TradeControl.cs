using System;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Plugins;

public readonly record struct TradeEventArgs(int Volume, TradeType TradeType, string SymbolName);

public class TradeControl : CustomControl
{
    private const string FontFamily = "Calibri";

    private readonly Grid _panel;
    private readonly ComboBox _tradeTypeComboBox;
    private readonly TextBox _volumeTextBox;

    public TradeControl()
    {
        _panel = new Grid(10, 2);

        _volumeTextBox = new TextBox
        {
            MinWidth = 200,
            FontFamily = FontFamily,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Margin = 3
        };

        _panel.AddChild(GetTextBlock("Volume"), 0, 0);
        _panel.AddChild(_volumeTextBox, 0, 1);

        _tradeTypeComboBox = new ComboBox
        {
            MinWidth = 200,
            FontFamily = FontFamily,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Margin = 3
        };

        _tradeTypeComboBox.AddItem("Buy");
        _tradeTypeComboBox.AddItem("Sell");

        _tradeTypeComboBox.SelectedItem = "Buy";

        var tradeTypeTextBlock = GetTextBlock("Trade Type");

        tradeTypeTextBlock.VerticalAlignment = VerticalAlignment.Center;

        _panel.AddChild(tradeTypeTextBlock, 1, 0);
        _panel.AddChild(_tradeTypeComboBox, 1, 1);

        var executeButton = new Button {Text = "Execute", FontFamily = FontFamily, BackgroundColor = Color.Red};

        executeButton.Click += ExecuteButtonOnClick;
        _panel.AddChild(executeButton, 2, 0, 1, 2);

        AddChild(_panel);
    }

    public Symbol Symbol { get; set; }
    
    public event EventHandler<TradeEventArgs> Trade;

    private void ExecuteButtonOnClick(ButtonClickEventArgs obj)
    {
        if (Symbol is null)
            return;
        
        if (!int.TryParse(_volumeTextBox.Text, out var volume))
            return;

        if (!Enum.TryParse(_tradeTypeComboBox.SelectedItem, out TradeType tradeType))
            return;

        Trade?.Invoke(this, new TradeEventArgs(volume, tradeType, Symbol.Name));
    }

    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 20,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}