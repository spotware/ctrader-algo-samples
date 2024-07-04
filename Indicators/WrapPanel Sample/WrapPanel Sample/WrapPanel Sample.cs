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
