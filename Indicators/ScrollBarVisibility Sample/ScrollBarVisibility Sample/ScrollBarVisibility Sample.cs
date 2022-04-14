using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use the ScrollBarVisibility
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ScrollBarVisibilitySample : Indicator
    {
        [Parameter("Horizontal Scroll Bar Visibility", DefaultValue = ScrollBarVisibility.Auto)]
        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; }

        [Parameter("Vertical Scroll Bar Visibility", DefaultValue = ScrollBarVisibility.Visible)]
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; }

        protected override void Initialize()
        {
            var scrollViewer = new ScrollViewer 
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.7,
                HorizontalScrollBarVisibility = HorizontalScrollBarVisibility,
                VerticalScrollBarVisibility = VerticalScrollBarVisibility,
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
