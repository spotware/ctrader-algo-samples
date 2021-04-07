using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use TextWrapping property to manage the text wrap
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TextWrappingSample : Indicator
    {
        [Parameter("Text", DefaultValue = "very long texttttttttttttttttttttt")]
        public string Text { get; set; }

        [Parameter("Wrapping", DefaultValue = TextWrapping.NoWrap)]
        public TextWrapping TextWrapping { get; set; }

        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 0.6,
                Width = 100
            };

            stackPanel.AddChild(new TextBlock
            {
                Text = Text,
                FontWeight = FontWeight.ExtraBold,
                ForegroundColor = Color.Blue,
                TextWrapping = TextWrapping
            });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}