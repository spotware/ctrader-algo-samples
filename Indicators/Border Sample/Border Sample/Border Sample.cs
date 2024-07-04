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
    public class BorderSample : Indicator
    {
        protected override void Initialize()
        {
            var border = new Border
            {
                BorderColor = Color.Yellow,
                BorderThickness = 2,
                Opacity = 0.5,
                BackgroundColor = Color.Violet,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Width = 200,
                Height = 100,
                Margin = 10
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            stackPanel.AddChild(new TextBlock
            {
                Text = "Text",
                Margin = 5,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeight.ExtraBold
            });

            stackPanel.AddChild(new Button
            {
                Text = "Button",
                Margin = 5,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeight.ExtraBold
            });

            stackPanel.AddChild(new TextBox
            {
                Text = "Type text...",
                Margin = 5,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontWeight = FontWeight.ExtraBold,
                Width = 100
            });

            border.Child = stackPanel;

            Chart.AddControl(border);
        }

        public override void Calculate(int index)
        {
        }
    }
}
