using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use the chart controls HorizontalAlignment to align your controls horizontally
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class HorizontalAlignmentSample : Indicator
    {
        [Parameter("Alignment", DefaultValue = HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                Width = 200,
                Height = 100
            };

            stackPanel.AddChild(new TextBlock
            {
                Text = "Text Block",
                ForegroundColor = Color.Black,
                FontWeight = FontWeight.ExtraBold,
                HorizontalAlignment = HorizontalAlignment
            });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}