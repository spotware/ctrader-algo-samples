using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use the WrapPanel
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class WrapPanelSample : Indicator
    {
        [Parameter("Panel Orientation", DefaultValue = Orientation.Vertical)]
        public Orientation PanelOrientation { get; set; }

        protected override void Initialize()
        {
            var wrapPanel = new WrapPanel
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Orientation = PanelOrientation,
                Width = 150,
                Height = 150
            };

            for (int i = 0; i < 10; i++)
            {
                wrapPanel.AddChild(new TextBlock
                {
                    Text = "Text",
                    Margin = 5,
                    ForegroundColor = Color.Black,
                    FontWeight = FontWeight.ExtraBold
                });
            }

            Chart.AddControl(wrapPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}