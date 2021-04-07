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

            stackPanel.AddChild(new TextBox
            {
                Text = "Enter text here...",
                FontWeight = FontWeight.ExtraBold,
                Margin = 5,
                ForegroundColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 150
            });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}