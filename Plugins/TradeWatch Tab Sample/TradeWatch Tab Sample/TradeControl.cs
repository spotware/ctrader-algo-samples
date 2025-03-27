// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//  
//    The sample creates a custom control for trading operations. The control provides a panel where
//    the user can specify the trade type (buy/sell) and the volume for a trade. When the "Execute" 
//    button is clicked, it triggers an event with the specified trade details (trade type, volume, symbol).
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Plugins;

// Declare a struct to hold trade event data.
public readonly record struct TradeEventArgs(int Volume, TradeType TradeType, string SymbolName);

// Declare a class to provide trading controls.
public class TradeControl : CustomControl
{
    private const string FontFamily = "Calibri";  // Define a constant for the font family to use in the control.

    private readonly Grid _panel;  // Create a panel for organising controls.
    private readonly ComboBox _tradeTypeComboBox;  // Add a ComboBox for selecting trade type.
    private readonly TextBox _volumeTextBox;  // Add a TextBox for inputting trade volume.

    // Define a method to initialise the custom control.
    public TradeControl()
    {
        _panel = new Grid(10, 2);  // Initialise a grid with specified rows and columns.

        _volumeTextBox = new TextBox  // Initialise the text box for volume input.
        {
            MinWidth = 200,
            FontFamily = FontFamily,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Margin = 3
        };

        _panel.AddChild(GetTextBlock("Volume"), 0, 0);  // Add a label to the grid.
        _panel.AddChild(_volumeTextBox, 0, 1);  // Add the volume text box to the grid.

        _tradeTypeComboBox = new ComboBox  // Initialise the combo box for trade type.
        {
            MinWidth = 200,
            FontFamily = FontFamily,
            FontSize = 20,
            FontWeight = FontWeight.Bold,
            Margin = 3
        };

        _tradeTypeComboBox.AddItem("Buy");  // Add the "Buy" option to the combo box.
        _tradeTypeComboBox.AddItem("Sell");  // Add the "Sell" option to the combo box.

        _tradeTypeComboBox.SelectedItem = "Buy";  // Set the default selected item.

        var tradeTypeTextBlock = GetTextBlock("Trade Type");  // Create a label for trade type.

        tradeTypeTextBlock.VerticalAlignment = VerticalAlignment.Center;  // Align the label vertically.

        _panel.AddChild(tradeTypeTextBlock, 1, 0);  // Add the label to the grid.
        _panel.AddChild(_tradeTypeComboBox, 1, 1);  // Add the combo box to the grid.

        var executeButton = new Button {Text = "Execute", FontFamily = FontFamily, BackgroundColor = Color.Red};  // Create a button for execution.

        executeButton.Click += ExecuteButtonOnClick;  // Attach the click event to the button.
        _panel.AddChild(executeButton, 2, 0, 1, 2);  // Add the button to the grid.

        AddChild(_panel);  // Add the panel to the custom control.
    }

    // Define a property to hold the symbol.
    public Symbol Symbol { get; set; }
    
    // Declare an event for trade execution.
    public event EventHandler<TradeEventArgs> Trade;

    // Define a method to handle the button click event.
    private void ExecuteButtonOnClick(ButtonClickEventArgs obj)
    {
        if (Symbol is null)  // Check if the symbol is null.
            return;
        
        if (!int.TryParse(_volumeTextBox.Text, out var volume))  // Validate the volume input.
            return;

        if (!Enum.TryParse(_tradeTypeComboBox.SelectedItem, out TradeType tradeType))  // Validate the trade type input.
            return;

        Trade?.Invoke(this, new TradeEventArgs(volume, tradeType, Symbol.Name));  // Trigger the trade event with the specified details.
    }

    // Define a method to create a text block with specific settings.
    private TextBlock GetTextBlock(string text = null) => new()
    {
        Margin = 3,
        FontSize = 20,
        FontWeight = FontWeight.Bold,
        FontFamily = FontFamily,
        Text = text
    };
}
