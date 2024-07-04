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
    public class ScrollViewerSample : Indicator
    {
        protected override void Initialize()
        {
            var scrollViewer = new ScrollViewer
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.7,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                Height = 100
            };

            var grid = new Grid(10, 2)
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            scrollViewer.Content = grid;

            for (int iRow = 0; iRow < 10; iRow++)
            {
                grid.AddChild(new TextBlock
                {
                    Text = "Text",
                    Margin = 5,
                    ForegroundColor = Color.Black,
                    FontWeight = FontWeight.ExtraBold
                }, iRow, 0);

                grid.AddChild(new Button
                {
                    Text = "Button",
                    Margin = 5,
                    ForegroundColor = Color.Black,
                    FontWeight = FontWeight.ExtraBold
                }, iRow, 1);
            }

            Chart.AddControl(scrollViewer);
        }

        public override void Calculate(int index)
        {
        }
    }
}
