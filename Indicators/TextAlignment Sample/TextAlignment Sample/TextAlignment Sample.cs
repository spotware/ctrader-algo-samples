using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use TextAlignment property to align the text
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TextAlignmentSample : Indicator
    {
        [Parameter("Text Alignment", DefaultValue = TextAlignment.Center)]
        public TextAlignment TextAlignment { get; set; }

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

            stackPanel.AddChild(new TextBlock { Text = "Sample text", TextAlignment = TextAlignment });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}