// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartColorSettingsSample : Indicator
    {
        private TextBox _askPriceLineColorTextBox;

        private TextBox _bidPriceLineColorTextBox;

        private TextBox _backgroundColorTextBox;

        protected override void Initialize()
        {
            var grid = new Grid(10, 2)
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            var style = new Style();

            style.Set(ControlProperty.Margin, 5);
            style.Set(ControlProperty.FontWeight, FontWeight.ExtraBold);
            style.Set(ControlProperty.ForegroundColor, Color.Red);
            style.Set(ControlProperty.MinWidth, 100);

            grid.AddChild(new TextBlock
            {
                Text = "Ask Price Line Color",
                Style = style
            }, 0, 0);

            _askPriceLineColorTextBox = new TextBox
            {
                Text = Chart.ColorSettings.AskPriceLineColor.ToString(),
                Style = style
            };

            grid.AddChild(_askPriceLineColorTextBox, 0, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Bid Price Line Color",
                Style = style
            }, 1, 0);

            _bidPriceLineColorTextBox = new TextBox
            {
                Text = Chart.ColorSettings.BidPriceLineColor.ToString(),
                Style = style
            };

            grid.AddChild(_bidPriceLineColorTextBox, 1, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Background Color",
                Style = style
            }, 2, 0);

            _backgroundColorTextBox = new TextBox
            {
                Text = Chart.ColorSettings.BackgroundColor.ToString(),
                Style = style
            };

            grid.AddChild(_backgroundColorTextBox, 2, 1);

            var changeButton = new Button
            {
                Text = "Change",
                Style = style
            };

            changeButton.Click += ChangeButton_Click;

            grid.AddChild(changeButton, 9, 0);

            Chart.AddControl(grid);
        }

        private void ChangeButton_Click(ButtonClickEventArgs obj)
        {
            Chart.ColorSettings.AskPriceLineColor = GetColor(_askPriceLineColorTextBox.Text);
            Chart.ColorSettings.BidPriceLineColor = GetColor(_bidPriceLineColorTextBox.Text);
            Chart.ColorSettings.BackgroundColor = GetColor(_backgroundColorTextBox.Text);
        }

        private Color GetColor(string colorString, int alpha = 255)
        {
            var color = colorString[0] == '#' ? Color.FromHex(colorString) : Color.FromName(colorString);

            return Color.FromArgb(alpha, color);
        }

        public override void Calculate(int index)
        {
        }
    }
}
