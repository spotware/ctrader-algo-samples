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
    public class TextChangedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 0.6
            };

            var textBox = new TextBox
            {
                Text = "Enter text here...",
                FontWeight = FontWeight.ExtraBold,
                Margin = 5,
                ForegroundColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 150
            };

            textBox.TextChanged += TextBox_TextChanged;

            stackPanel.AddChild(textBox);

            Chart.AddControl(stackPanel);
        }

        private void TextBox_TextChanged(TextChangedEventArgs obj)
        {
            Print("Text box text changed to: ", obj.TextBox.Text);
        }

        public override void Calculate(int index)
        {
        }
    }
}
