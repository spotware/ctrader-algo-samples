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
    public class StyleSample : Indicator
    {
        protected override void Initialize()
        {
            var style = new Style();

            style.Set(ControlProperty.Margin, 5);
            style.Set(ControlProperty.ForegroundColor, Color.Blue);
            style.Set(ControlProperty.FontSize, 14);
            style.Set(ControlProperty.Width, 100);

            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Orientation = Orientation.Vertical
            };

            for (var i = 0; i < 10; i++)
            {
                stackPanel.AddChild(new TextBlock
                {
                    Text = "Text Block #" + i,
                    Style = style
                });
            }

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
