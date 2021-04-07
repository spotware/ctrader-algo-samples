using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to add a text block control on your chart
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TextBlockSample : Indicator
    {
        [Parameter("Text", DefaultValue = "Sample text")]
        public string Text { get; set; }

        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 0.6,
                Width = 200
            };

            stackPanel.AddChild(new TextBlock { Text = Text, FontWeight = FontWeight.ExtraBold, ForegroundColor = Color.Blue });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}