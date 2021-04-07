using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to add a text box control on your chart
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TextBoxSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 0.6,
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