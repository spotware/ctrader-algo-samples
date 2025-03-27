// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The sample demonstrates how to create a custom plugin that allows the user to manage trading 
//    margin by displaying available margin, adjusting the margin to trade and executing buy/sell 
//    trades based on user input. It also includes buttons for increasing, decreasing and setting 
//    the maximum margin for trading.
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
    public class OrderByMargin : Plugin
    {
        TextBlock availableMarginTextBlock = new TextBlock();  // Create a TextBlock control to display available margin information.
        TextBlock quantityToTradeTextBlock = new TextBlock();  // Create a TextBlock control to display the quantity to trade.
        ViewModel viewModel = new ViewModel();  // Create a ViewModel instance to manage the data and events related to margin and trade quantity.
        TextBox tradeableMarginTextBox;  // Declare a TextBox control that will be used to display the margin available for trading.

        // This method is triggered when the plugin starts.        
        protected override void OnStart()
        {
            viewModel.Changed += viewModel_Changed;  // Event handler for viewModel changes.

            AddControls();  // Call method to add UI controls to the plugin interface.

            // Subscribe to position events to update margin when positions are modified.
            Positions.Opened += Positions_Opened;
            Positions.Closed += Positions_Closed;
            Positions.Modified += Positions_Modified;
            Asp.SymbolTab.SymbolChanged += AspSymbolTab_SymbolChanged;
            Account.Switched += Account_Switched;

            UpdateAvailableMargin();  // Update available margin on start.
            SetMaximumMargin();  // Set the maximum margin on start.
        }

        // This method handles accounts switching, triggering a recalculation of the maximum margin.
        private void Account_Switched(AccountSwitchedEventArgs obj)
        {
            SetMaximumMargin();  // Update maximum margin when account is switched.
        }

        // This method handles symbols changing, triggering a recalculation of the maximum margin.
        private void AspSymbolTab_SymbolChanged(AspSymbolChangedEventArgs obj)
        {
            SetMaximumMargin();  // Update maximum margin when symbol is changed.
        }

        // This method builds the plugin interface with labels, buttons and input fields. It includes margin control buttons and trade buttons.
        private void AddControls()
        {
            // Create a new block for the plugin interface.
            var block = Asp.SymbolTab.AddBlock("New Order by Margin");
            block.IsExpanded = true;
            block.IsDetachable = false;
            block.Index = 1;
            block.Height = 150;

            // Create a stack panel with margins.
            var rootStackPanel = new StackPanel { Margin = new Thickness(10) };

            // Create and arrange UI elements to display available margin in a horizontal layout.
            var availableMarginStackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            availableMarginStackPanel.AddChild(new TextBlock { Text = "Available Margin: " });
            availableMarginStackPanel.AddChild(availableMarginTextBlock);
            rootStackPanel.AddChild(availableMarginStackPanel);

            // Label for margin to trade.
            rootStackPanel.AddChild(new TextBlock { Text = "Margin to trade:", Margin = new Thickness(0, 10, 0, 0) });

            // Create a grid layout for organizing margin controls, defining columns with specific width settings.
            var tradeMarginGrid = new Grid { Margin = new Thickness(10) };
            tradeMarginGrid.AddColumn().SetWidthInStars(1);
            tradeMarginGrid.AddColumn().SetWidthToAuto();
            tradeMarginGrid.AddColumn().SetWidthToAuto();
            tradeMarginGrid.AddColumn().SetWidthToAuto();

            // Add the text box for tradeable margin with styles.
            tradeableMarginTextBox = new TextBox { IsReadOnly = true, TextAlignment = TextAlignment.Right };
            var tradeableMarginTextBoxStyle = new Style();
            tradeableMarginTextBoxStyle.Set(ControlProperty.BackgroundColor, Color.FromArgb(26, 26, 26), ControlState.DarkTheme);
            tradeableMarginTextBoxStyle.Set(ControlProperty.ForegroundColor, Color.FromArgb(255, 255, 255), ControlState.DarkTheme);
            tradeableMarginTextBoxStyle.Set(ControlProperty.BackgroundColor, Color.FromArgb(231, 235, 237), ControlState.LightTheme);
            tradeableMarginTextBoxStyle.Set(ControlProperty.ForegroundColor, Color.FromArgb(55, 56, 57), ControlState.LightTheme);
            tradeableMarginTextBox.Style = tradeableMarginTextBoxStyle;
            tradeMarginGrid.AddChild(tradeableMarginTextBox, 0, 0);
            
            // Buttons for increasing, decreasing and setting maximum margin.
            var decreaseMarginButton = new Button { Text = "-", Margin = new Thickness(5, 0, 5, 0) };
            decreaseMarginButton.Click += decreaseMarginButton_Click;
            tradeMarginGrid.AddChild(decreaseMarginButton, 0, 1);
            var increaseMarginButton = new Button { Text = "+" };
            increaseMarginButton.Click += increaseMarginButton_Click;
            tradeMarginGrid.AddChild(increaseMarginButton, 0, 2);
            var maxMarginButton = new Button { Text = "max", Margin = new Thickness(5, 0, 0, 0), VerticalAlignment = VerticalAlignment.Stretch };
            maxMarginButton.Click += maxMarginButton_Click; ;
            tradeMarginGrid.AddChild(maxMarginButton, 0, 3);
            rootStackPanel.AddChild(tradeMarginGrid);

            // Add quantity to trade section.
            var volumeStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            volumeStackPanel.AddChild(new TextBlock { Text = "Quantity to trade: " });
            volumeStackPanel.AddChild(quantityToTradeTextBlock);
            volumeStackPanel.AddChild(new TextBlock { Text = " Lots" });
            rootStackPanel.AddChild(volumeStackPanel);

            // Add trade buttons for Buy and Sell with styles.
            var tradeButtons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center };
            tradeButtons.AddChild(CreateTradeButton("Sell", Styles.CreateSellButtonStyle(), TradeType.Sell));
            tradeButtons.AddChild(CreateTradeButton("Buy", Styles.CreateBuyButtonStyle(), TradeType.Buy));
            rootStackPanel.AddChild(tradeButtons);

            // Add the root stack panel to the block.
            block.Child = rootStackPanel;
        }

        // Create a trade button for executing either a buy or sell trade with specific styles.
        private Button CreateTradeButton(string text, Style style, TradeType tradeType)
        {
            var tradeButton = new Button
            {
                Text = text,
                Style = style,
                Height = 25
            };

            // Set button click action to execute a market order.
            tradeButton.Click += args => ExecuteMarketOrderAsync(tradeType, Asp.SymbolTab.Symbol.Name, viewModel.Quantity * Asp.SymbolTab.Symbol.LotSize);

            return tradeButton;
        }

        // Event handler for when the "max" margin button is clicked, setting the margin to the maximum possible value.
        private void maxMarginButton_Click(ButtonClickEventArgs obj)
        {
            SetMaximumMargin();  // Update margin to the maximum available.
        }

        // Event handler for when the "increase" margin button is clicked, increasing the margin based on the available quantity.
        private void increaseMarginButton_Click(ButtonClickEventArgs obj)
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);

            // Check if the quantity exceeds maximum allowed volume.
            if (viewModel.Quantity > symbol.VolumeInUnitsMax / symbol.LotSize)
                return;

            viewModel.Quantity += symbol.VolumeInUnitsMin / symbol.LotSize;  // Increase quantity.
            RecalculateMargin(viewModel.Quantity);  // Recalculate margin for updated quantity.
        }

        // Event handler for when the "decrease" margin button is clicked, decreasing the margin based on the available quantity.
        private void decreaseMarginButton_Click(ButtonClickEventArgs obj)
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);

            // Ensure quantity does not go below the minimum allowed.
            if (viewModel.Quantity <= symbol.VolumeInUnitsMin / symbol.LotSize)
                return;

            viewModel.Quantity -= symbol.VolumeInUnitsMin / symbol.LotSize;  // Decrease quantity.
            RecalculateMargin(viewModel.Quantity);  // Recalculate margin for updated quantity.
        }

        // Update the displayed margin-related values whenever the view model is changed.
        private void viewModel_Changed()
        {
            availableMarginTextBlock.Text = Math.Floor(viewModel.AvailableMargin).ToString() + " " + Account.Asset.Name;
            quantityToTradeTextBlock.Text = Math.Round(viewModel.Quantity, 2).ToString();
            tradeableMarginTextBox.Text = viewModel.MarginToTrade.ToString();
        }

        // Event handler for when a position is modified, updating the available margin.
        private void Positions_Modified(PositionModifiedEventArgs obj)
        {
            UpdateAvailableMargin();
        }

        // Event handler for when a position is closed, updating the available margin.
        private void Positions_Closed(PositionClosedEventArgs obj)
        {
            UpdateAvailableMargin();
        }

        // Event handler for when a position is opened, updating the available margin.
        private void Positions_Opened(PositionOpenedEventArgs obj)
        {
            UpdateAvailableMargin();
        }

        // Update the available margin based on the free margin in the account.
        private void UpdateAvailableMargin()
        {
            viewModel.AvailableMargin = Account.FreeMargin;
        }

        // Set the margin to the maximum value based on available free margin and leverage.
        private void SetMaximumMargin()
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);
            var volume = Account.Asset.Convert(symbol.BaseAsset, Account.FreeMargin * leverage);
            var tradeableVolume = symbol.NormalizeVolumeInUnits(volume, RoundingMode.Down);

            viewModel.Quantity = tradeableVolume / symbol.LotSize;
            viewModel.MarginToTrade = symbol.BaseAsset.Convert(Account.Asset, tradeableVolume / leverage);
        }
        
        // Recalculate the margin required based on the provided quantity.
        private void RecalculateMargin(double quantity)
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);

            var volume = quantity * symbol.LotSize;
            var margin = symbol.BaseAsset.Convert(Account.Asset, volume / leverage);
            viewModel.MarginToTrade = Math.Floor(margin);
        }
    }

    // A static class containing methods for defining button styles used in the application.
    public static class Styles
    {
        // Create the style for a "Buy" button with a specific background and hover color.
        public static Style CreateBuyButtonStyle()
        {
            return CreateButtonStyle(Color.FromHex("#009345"), Color.FromHex("#10A651"));
        }

        // Create the style for a "Sell" button with a specific background and hover color.
        public static Style CreateSellButtonStyle()
        {
            return CreateButtonStyle(Color.FromHex("#F05824"), Color.FromHex("#FF6C36"));
        }

        // A helper method to create button styles, defining the background and hover colors, foreground color, width and margin.
        private static Style CreateButtonStyle(Color color, Color hoverColor)
        {
            var style = new Style(DefaultStyles.ButtonStyle);
            style.Set(ControlProperty.BackgroundColor, color, ControlState.DarkTheme);
            style.Set(ControlProperty.BackgroundColor, color, ControlState.LightTheme);
            style.Set(ControlProperty.BackgroundColor, hoverColor, ControlState.DarkTheme | ControlState.Hover);
            style.Set(ControlProperty.BackgroundColor, hoverColor, ControlState.LightTheme | ControlState.Hover);
            style.Set(ControlProperty.ForegroundColor, Color.FromHex("#FFFFFF"), ControlState.DarkTheme);
            style.Set(ControlProperty.ForegroundColor, Color.FromHex("#FFFFFF"), ControlState.LightTheme);
            style.Set(ControlProperty.Width, 100);
            style.Set(ControlProperty.Margin, new Thickness(5, 0, 5, 0));
            return style;
        }
    }

    // ViewModel class that holds the data properties and notifies changes to the UI when the data is updated.
    class ViewModel
    {
        private double _availableMargin;  // Private backing field for AvailableMargin.
        public double AvailableMargin  // Public property for AvailableMargin with a getter and setter.
        {
            get { return _availableMargin; }  // Return the current value of _availableMargin.
            set
            {
                if (value == _availableMargin)  // If the new value is the same as the current one, no action is taken.
                    return;
                _availableMargin = value;  // Update the private backing field with the new value.

                Changed?.Invoke();  // Trigger the Changed event to notify subscribers of the property change.
            }
        }

        private double _quantity;  // Private backing field for Quantity.
        public double Quantity  // Public property for Quantity with a getter and setter.
        {
            get { return _quantity; }  // Return the current value of _quantity.
            set
            {
                if (value == _quantity)  // If the new value is the same as the current one, no action is taken.
                    return;
                _quantity = value;  // Update the private backing field with the new value.

                Changed?.Invoke();  // Trigger the Changed event to notify subscribers of the property change.
            }
        }

        private double _marginToTrade;  // Private backing field for MarginToTrade.
        public double MarginToTrade  // Public property for MarginToTrade with a getter and setter.
        {
            get { return _marginToTrade; }  // Return the current value of _marginToTrade.
            set
            {
                if (value == _marginToTrade)  // If the new value is the same as the current one, no action is taken.
                    return;
                _marginToTrade = value;  // Update the private backing field with the new value.

                Changed?.Invoke();  // Trigger the Changed event to notify subscribers of the property change.
            }
        }

        // Event that is triggered whenever any of the properties change.
        public event Action Changed;
    }
}
