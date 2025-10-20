// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
//    The "Sample Trading Panel" allows you to execute trades via algo controls.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleTradingPanel : Robot
    {
        [Parameter("Vertical Position", Group = "Panel alignment", DefaultValue = VerticalAlignment.Top)]
        public VerticalAlignment PanelVerticalAlignment { get; set; }

        [Parameter("Horizontal Position", Group = "Panel alignment", DefaultValue = HorizontalAlignment.Left)]
        public HorizontalAlignment PanelHorizontalAlignment { get; set; }

        [Parameter("Default Lots", Group = "Default trade parameters", DefaultValue = 0.01)]
        public double DefaultLots { get; set; }

        [Parameter("Default Take Profit (pips)", Group = "Default trade parameters", DefaultValue = 20)]
        public double DefaultTakeProfitPips { get; set; }

        [Parameter("Default Stop Loss (pips)", Group = "Default trade parameters", DefaultValue = 20)]
        public double DefaultStopLossPips { get; set; }

        protected override void OnStart()
        {
            var tradingPanel = new TradingPanel(this, Symbol, DefaultLots, DefaultStopLossPips, DefaultTakeProfitPips);

            var border = new Border 
            {
                VerticalAlignment = PanelVerticalAlignment,
                HorizontalAlignment = PanelHorizontalAlignment,
                Style = Styles.CreatePanelBackgroundStyle(),
                Margin = "20 40 20 20",
                Width = 225,
                Child = tradingPanel
            };

            Chart.AddControl(border);
        }
    }

    public class TradingPanel : CustomControl
    {
        private const string LotsInputKey = "LotsKey";
        private const string TakeProfitInputKey = "TPKey";
        private const string StopLossInputKey = "SLKey";
        private readonly IDictionary<string, TextBox> _inputMap = new Dictionary<string, TextBox>();
        private readonly Robot _robot;
        private readonly Symbol _symbol;

        public TradingPanel(Robot robot, Symbol symbol, double defaultLots, double defaultStopLossPips, double defaultTakeProfitPips)
        {
            _robot = robot;
            _symbol = symbol;
            AddChild(CreateTradingPanel(defaultLots, defaultStopLossPips, defaultTakeProfitPips));
        }

        private ControlBase CreateTradingPanel(double defaultLots, double defaultStopLossPips, double defaultTakeProfitPips)
        {
            var mainPanel = new StackPanel();

            var header = CreateHeader();
            mainPanel.AddChild(header);

            var contentPanel = CreateContentPanel(defaultLots, defaultStopLossPips, defaultTakeProfitPips);
            mainPanel.AddChild(contentPanel);

            return mainPanel;
        }

        private ControlBase CreateHeader()
        {
            var headerBorder = new Border 
            {
                BorderThickness = "0 0 0 1",
                Style = Styles.CreateCommonBorderStyle()
            };

            var header = new TextBlock 
            {
                Text = "Quick Trading Panel",
                Margin = "10 7",
                Style = Styles.CreateHeaderStyle()
            };

            headerBorder.Child = header;
            return headerBorder;
        }

        private StackPanel CreateContentPanel(double defaultLots, double defaultStopLossPips, double defaultTakeProfitPips)
        {
            var contentPanel = new StackPanel 
            {
                Margin = 10
            };
            var grid = new Grid(4, 3);
            grid.Columns[1].SetWidthInPixels(5);

            var sellButton = CreateTradeButton("SELL", Styles.CreateSellButtonStyle(), TradeType.Sell);
            grid.AddChild(sellButton, 0, 0);

            var buyButton = CreateTradeButton("BUY", Styles.CreateBuyButtonStyle(), TradeType.Buy);
            grid.AddChild(buyButton, 0, 2);

            var lotsInput = CreateInputWithLabel("Quantity (Lots)", defaultLots.ToString("F2"), LotsInputKey);
            grid.AddChild(lotsInput, 1, 0, 1, 3);

            var stopLossInput = CreateInputWithLabel("Stop Loss (Pips)", defaultStopLossPips.ToString("F1"), StopLossInputKey);
            grid.AddChild(stopLossInput, 2, 0);

            var takeProfitInput = CreateInputWithLabel("Take Profit (Pips)", defaultTakeProfitPips.ToString("F1"), TakeProfitInputKey);
            grid.AddChild(takeProfitInput, 2, 2);

            var closeAllButton = CreateCloseAllButton();
            grid.AddChild(closeAllButton, 3, 0, 1, 3);

            contentPanel.AddChild(grid);

            return contentPanel;
        }

        private Button CreateTradeButton(string text, Style style, TradeType tradeType)
        {
            var tradeButton = new Button 
            {
                Text = text,
                Style = style,
                Height = 25
            };

            tradeButton.Click += args => ExecuteMarketOrderAsync(tradeType);

            return tradeButton;
        }

        private ControlBase CreateCloseAllButton()
        {
            var closeAllBorder = new Border 
            {
                Margin = "0 10 0 0",
                BorderThickness = "0 1 0 0",
                Style = Styles.CreateCommonBorderStyle()
            };

            var closeButton = new Button 
            {
                Style = Styles.CreateCloseButtonStyle(),
                Text = "Close All",
                Margin = "0 10 0 0"
            };

            closeButton.Click += args => CloseAll();
            closeAllBorder.Child = closeButton;

            return closeAllBorder;
        }

        private Panel CreateInputWithLabel(string label, string defaultValue, string inputKey)
        {
            var stackPanel = new StackPanel 
            {
                Orientation = Orientation.Vertical,
                Margin = "0 10 0 0"
            };

            var textBlock = new TextBlock 
            {
                Text = label
            };

            var input = new TextBox 
            {
                Margin = "0 5 0 0",
                Text = defaultValue,
                Style = Styles.CreateInputStyle()
            };

            _inputMap.Add(inputKey, input);

            stackPanel.AddChild(textBlock);
            stackPanel.AddChild(input);

            return stackPanel;
        }

        private void ExecuteMarketOrderAsync(TradeType tradeType)
        {
            var lots = GetValueFromInput(LotsInputKey, 0);
            if (lots <= 0)
            {
                _robot.Print(string.Format("{0} failed, invalid Lots", tradeType));
                return;
            }

            var stopLossPips = GetValueFromInput(StopLossInputKey, 0);
            var takeProfitPips = GetValueFromInput(TakeProfitInputKey, 0);

            _robot.Print(string.Format("Open position with: LotsParameter: {0}, StopLossPipsParameter: {1}, TakeProfitPipsParameter: {2}", lots, stopLossPips, takeProfitPips));

            var volume = _symbol.QuantityToVolumeInUnits(lots);
            _robot.ExecuteMarketOrderAsync(tradeType, _symbol.Name, volume, "Trade Panel Sample", stopLossPips, takeProfitPips);
        }

        private double GetValueFromInput(string inputKey, double defaultValue)
        {
            double value;

            return double.TryParse(_inputMap[inputKey].Text, out value) ? value : defaultValue;
        }

        private void CloseAll()
        {
            foreach (var position in _robot.Positions)
                _robot.ClosePositionAsync(position);
        }
    }

    public static class Styles
    {
        public static Style CreatePanelBackgroundStyle()
        {
            var style = new Style();
            style.Set(ControlProperty.CornerRadius, 3);
            style.Set(ControlProperty.BackgroundColor, GetColorWithOpacity(Color.FromHex("#292929"), 0.85m), ControlState.DarkTheme);
            style.Set(ControlProperty.BackgroundColor, GetColorWithOpacity(Color.FromHex("#FFFFFF"), 0.85m), ControlState.LightTheme);
            style.Set(ControlProperty.BorderColor, Color.FromHex("#3C3C3C"), ControlState.DarkTheme);
            style.Set(ControlProperty.BorderColor, Color.FromHex("#C3C3C3"), ControlState.LightTheme);
            style.Set(ControlProperty.BorderThickness, new Thickness(1));

            return style;
        }

        public static Style CreateCommonBorderStyle()
        {
            var style = new Style();
            style.Set(ControlProperty.BorderColor, GetColorWithOpacity(Color.FromHex("#FFFFFF"), 0.12m), ControlState.DarkTheme);
            style.Set(ControlProperty.BorderColor, GetColorWithOpacity(Color.FromHex("#000000"), 0.12m), ControlState.LightTheme);
            return style;
        }

        public static Style CreateHeaderStyle()
        {
            var style = new Style();
            style.Set(ControlProperty.ForegroundColor, GetColorWithOpacity("#FFFFFF", 0.70m), ControlState.DarkTheme);
            style.Set(ControlProperty.ForegroundColor, GetColorWithOpacity("#000000", 0.65m), ControlState.LightTheme);
            return style;
        }

        public static Style CreateInputStyle()
        {
            var style = new Style(DefaultStyles.TextBoxStyle);
            style.Set(ControlProperty.BackgroundColor, Color.FromHex("#1A1A1A"), ControlState.DarkTheme);
            style.Set(ControlProperty.BackgroundColor, Color.FromHex("#111111"), ControlState.DarkTheme | ControlState.Hover);
            style.Set(ControlProperty.BackgroundColor, Color.FromHex("#E7EBED"), ControlState.LightTheme);
            style.Set(ControlProperty.BackgroundColor, Color.FromHex("#D6DADC"), ControlState.LightTheme | ControlState.Hover);
            style.Set(ControlProperty.CornerRadius, 3);
            return style;
        }

        public static Style CreateBuyButtonStyle()
        {
            return CreateButtonStyle(Color.FromHex("#009345"), Color.FromHex("#10A651"));
        }

        public static Style CreateSellButtonStyle()
        {
            return CreateButtonStyle(Color.FromHex("#F05824"), Color.FromHex("#FF6C36"));
        }

        public static Style CreateCloseButtonStyle()
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
            return style;
        }

        private static Color GetColorWithOpacity(Color baseColor, decimal opacity)
        {
            var alpha = (int)Math.Round(byte.MaxValue * opacity, MidpointRounding.AwayFromZero);
            return Color.FromArgb(alpha, baseColor);
        }
    }
}
