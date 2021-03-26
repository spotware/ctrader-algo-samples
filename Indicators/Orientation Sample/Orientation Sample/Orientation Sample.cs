using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to set stack and wrap panels orientation
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class OrientationSample : Indicator
    {
        [Parameter("Orientation", DefaultValue = Orientation.Vertical)]
        public Orientation Orientation { get; set; }

        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.7,
            };

            stackPanel.AddChild(new TextBlock { Text = "First TextBlock", FontWeight = FontWeight.ExtraBold, Margin = 5, ForegroundColor = Color.Black });
            stackPanel.AddChild(new TextBlock { Text = "Second TextBlock", FontWeight = FontWeight.ExtraBold, Margin = 5, ForegroundColor = Color.Black });
            stackPanel.AddChild(new TextBlock { Text = "Third TextBlock", FontWeight = FontWeight.ExtraBold, Margin = 5, ForegroundColor = Color.Black });
            stackPanel.AddChild(new TextBlock { Text = "Fourth TextBlock", FontWeight = FontWeight.ExtraBold, Margin = 5, ForegroundColor = Color.Black });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}