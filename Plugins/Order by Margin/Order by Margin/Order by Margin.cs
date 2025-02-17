// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
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
    public class OrderByMargin : Plugin
    {
        TextBlock availableMarginTextBlock = new TextBlock();
        TextBlock quantityToTradeTextBlock = new TextBlock();
        ViewModel viewModel = new ViewModel();
        TextBox tradeableMarginTextBox;

        protected override void OnStart()
        {
            viewModel.Changed += viewModel_Changed;

            AddControls();

            Positions.Opened += Positions_Opened;
            Positions.Closed += Positions_Closed;
            Positions.Modified += Positions_Modified;
            Asp.SymbolTab.SymbolChanged += AspSymbolTab_SymbolChanged;
            Account.Switched += Account_Switched;

            UpdateAvailableMargin();
            SetMaximumMargin();
        }

        private void Account_Switched(AccountSwitchedEventArgs obj)
        {
            SetMaximumMargin();
        }

        private void AspSymbolTab_SymbolChanged(AspSymbolChangedEventArgs obj)
        {
            SetMaximumMargin();
        }

        private void AddControls()
        {
            var block = Asp.SymbolTab.AddBlock("New Order by Margin");

            block.IsDetachable = false;
            block.Height = 150;

            var rootStackPanel = new StackPanel { Margin = new Thickness(10) };

            var availableMarginStackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            availableMarginStackPanel.AddChild(new TextBlock { Text = "Available Margin: " });
            availableMarginStackPanel.AddChild(availableMarginTextBlock);
            rootStackPanel.AddChild(availableMarginStackPanel);

            rootStackPanel.AddChild(new TextBlock { Text = "Margin to trade:", Margin = new Thickness(0, 10, 0, 0) });

            var tradeMarginGrid = new Grid { Margin = new Thickness(10) };
            tradeMarginGrid.AddColumn().SetWidthInStars(1);
            tradeMarginGrid.AddColumn().SetWidthToAuto();
            tradeMarginGrid.AddColumn().SetWidthToAuto();
            tradeMarginGrid.AddColumn().SetWidthToAuto();

            tradeableMarginTextBox = new TextBox { IsReadOnly = true, TextAlignment = TextAlignment.Right };
            var tradeableMarginTextBoxStyle = new Style();
            tradeableMarginTextBoxStyle.Set(ControlProperty.BackgroundColor, Color.FromArgb(26, 26, 26), ControlState.DarkTheme);
            tradeableMarginTextBoxStyle.Set(ControlProperty.ForegroundColor, Color.FromArgb(255, 255, 255), ControlState.DarkTheme);
            tradeableMarginTextBoxStyle.Set(ControlProperty.BackgroundColor, Color.FromArgb(231, 235, 237), ControlState.LightTheme);
            tradeableMarginTextBoxStyle.Set(ControlProperty.ForegroundColor, Color.FromArgb(55, 56, 57), ControlState.LightTheme);
            tradeableMarginTextBox.Style = tradeableMarginTextBoxStyle;
            tradeMarginGrid.AddChild(tradeableMarginTextBox, 0, 0);
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

            var volumeStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            volumeStackPanel.AddChild(new TextBlock { Text = "Quantity to trade: " });
            volumeStackPanel.AddChild(quantityToTradeTextBlock);
            volumeStackPanel.AddChild(new TextBlock { Text = " Lots" });
            rootStackPanel.AddChild(volumeStackPanel);

            var tradeButtons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10), HorizontalAlignment = HorizontalAlignment.Center };
            tradeButtons.AddChild(CreateTradeButton("Sell", Styles.CreateSellButtonStyle(), TradeType.Sell));
            tradeButtons.AddChild(CreateTradeButton("Buy", Styles.CreateBuyButtonStyle(), TradeType.Buy));
            rootStackPanel.AddChild(tradeButtons);

            block.Child = rootStackPanel;
        }

        private Button CreateTradeButton(string text, Style style, TradeType tradeType)
        {
            var tradeButton = new Button
            {
                Text = text,
                Style = style,
                Height = 25
            };

            tradeButton.Click += args => ExecuteMarketOrderAsync(tradeType, Asp.SymbolTab.Symbol.Name, viewModel.Quantity * Asp.SymbolTab.Symbol.LotSize);

            return tradeButton;
        }


        private void maxMarginButton_Click(ButtonClickEventArgs obj)
        {
            SetMaximumMargin();
        }

        private void increaseMarginButton_Click(ButtonClickEventArgs obj)
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);

            if (viewModel.Quantity > symbol.VolumeInUnitsMax / symbol.LotSize)
                return;

            viewModel.Quantity += symbol.VolumeInUnitsMin / symbol.LotSize;
            RecalculateMargin(viewModel.Quantity);
        }

        private void decreaseMarginButton_Click(ButtonClickEventArgs obj)
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);

            if (viewModel.Quantity <= symbol.VolumeInUnitsMin / symbol.LotSize)
                return;

            viewModel.Quantity -= symbol.VolumeInUnitsMin / symbol.LotSize;
            RecalculateMargin(viewModel.Quantity);
        }

        private void viewModel_Changed()
        {
            availableMarginTextBlock.Text = Math.Floor(viewModel.AvailableMargin).ToString() + " " + Account.Asset.Name;
            quantityToTradeTextBlock.Text = Math.Round(viewModel.Quantity, 2).ToString();
            tradeableMarginTextBox.Text = viewModel.MarginToTrade.ToString();
        }

        private void Positions_Modified(PositionModifiedEventArgs obj)
        {
            UpdateAvailableMargin();
        }

        private void Positions_Closed(PositionClosedEventArgs obj)
        {
            UpdateAvailableMargin();
        }

        private void Positions_Opened(PositionOpenedEventArgs obj)
        {
            UpdateAvailableMargin();
        }

        private void UpdateAvailableMargin()
        {
            viewModel.AvailableMargin = Account.FreeMargin;
        }

        private void SetMaximumMargin()
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);
            var volume = Account.Asset.Convert(symbol.BaseAsset, Account.FreeMargin * leverage);
            var tradeableVolume = symbol.NormalizeVolumeInUnits(volume, RoundingMode.Down);

            viewModel.Quantity = tradeableVolume / symbol.LotSize;
            viewModel.MarginToTrade = symbol.BaseAsset.Convert(Account.Asset, tradeableVolume / leverage);
        }
        private void RecalculateMargin(double quantity)
        {
            var symbol = Asp.SymbolTab.Symbol;
            var leverage = Math.Min(symbol.DynamicLeverage[0].Leverage, Account.PreciseLeverage);

            var volume = quantity * symbol.LotSize;
            var margin = symbol.BaseAsset.Convert(Account.Asset, volume / leverage);
            viewModel.MarginToTrade = Math.Floor(margin);
        }
    }

    public static class Styles
    {
        public static Style CreateBuyButtonStyle()
        {
            return CreateButtonStyle(Color.FromHex("#009345"), Color.FromHex("#10A651"));
        }

        public static Style CreateSellButtonStyle()
        {
            return CreateButtonStyle(Color.FromHex("#F05824"), Color.FromHex("#FF6C36"));
        }

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


    class ViewModel
    {
        private double _availableMargin;
        public double AvailableMargin
        {
            get { return _availableMargin; }
            set
            {
                if (value == _availableMargin)
                    return;
                _availableMargin = value;

                Changed?.Invoke();
            }
        }

        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                if (value == _quantity)
                    return;
                _quantity = value;

                Changed?.Invoke();
            }
        }

        private double _marginToTrade;
        public double MarginToTrade
        {
            get { return _marginToTrade; }
            set
            {
                if (value == _marginToTrade)
                    return;
                _marginToTrade = value;

                Changed?.Invoke();
            }
        }


        public event Action Changed;
    }
}