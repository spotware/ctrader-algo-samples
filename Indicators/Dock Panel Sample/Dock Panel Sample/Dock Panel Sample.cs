// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
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

            dockPanel.AddChild(new TextBox
            {
                Dock = Dock.Bottom,
                Margin = 5,
                Width = 100
            });

            Chart.AddControl(dockPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
