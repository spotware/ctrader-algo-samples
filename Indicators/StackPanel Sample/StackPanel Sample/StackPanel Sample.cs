using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use the StackPanel
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class StackPanelSample : Indicator
    {
        [Parameter("Panel Orientation", DefaultValue = Orientation.Vertical)]
        public Orientation PanelOrientation { get; set; }

        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Orientation = PanelOrientation
            };

            for (int i = 0; i < 10; i++)
            {
                stackPanel.AddChild(new TextBlock
                {
                    Text = "Text",
                    Margin = 5,
                    ForegroundColor = Color.Black,
                    FontWeight = FontWeight.ExtraBold
                });
            }

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}