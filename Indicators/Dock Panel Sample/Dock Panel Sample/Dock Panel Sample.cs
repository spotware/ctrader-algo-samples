using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use DockPanel
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class DockPanelSample : Indicator
    {
        protected override void Initialize()
        {
            var dockPanel = new DockPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.8
            };

            dockPanel.AddChild(new TextBlock
            {
                Text = "Enter Your Name",
                Margin = 5,
                Dock = Dock.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                ForegroundColor = Color.Black,
                FontWeight = FontWeight.ExtraBold
            });

            dockPanel.AddChild(new TextBox { Dock = Dock.Bottom, Margin = 5, Width = 100 });

            Chart.AddControl(dockPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}