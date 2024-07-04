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
    public class RectangleShapeSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.6
            };

            var rectangle = new Rectangle
            {
                StrokeColor = Color.Blue,
                FillColor = Color.Red,
                StrokeThickness = 2,
                Margin = 10,
                Width = 300,
                Height = 100
            };

            stackPanel.AddChild(rectangle);

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
