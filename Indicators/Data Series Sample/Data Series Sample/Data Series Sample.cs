using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to work with data series
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class DataSeriesSample : Indicator
    {
        private TextBlock _lastValueTextBlock;
        private TextBlock _lastClosedValueTextBlock;
        private TextBlock _countTextBlock;

        [Parameter()]
        public DataSeries Source { get; set; }

        protected override void Initialize()
        {
            var grid = new Grid(3, 2) 
            {
                BackgroundColor = Color.DarkGoldenrod,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                Opacity = 0.5
            };

            grid.AddChild(new TextBlock 
            {
                Text = "Last Value",
                Margin = 5
            }, 0, 0);

            _lastValueTextBlock = new TextBlock 
            {
                Text = Source.LastValue.ToString(),
                Margin = 5
            };

            grid.AddChild(_lastValueTextBlock, 0, 1);

            grid.AddChild(new TextBlock 
            {
                Text = "Last Closed Value",
                Margin = 5
            }, 1, 0);

            _lastClosedValueTextBlock = new TextBlock 
            {
                Text = Source.Last(1).ToString(),
                Margin = 5
            };

            grid.AddChild(_lastClosedValueTextBlock, 1, 1);

            grid.AddChild(new TextBlock 
            {
                Text = "Values Count",
                Margin = 5
            }, 2, 0);

            _countTextBlock = new TextBlock 
            {
                Text = Source.Count.ToString(),
                Margin = 5
            };

            grid.AddChild(_countTextBlock, 2, 1);

            Chart.AddControl(grid);
        }

        public override void Calculate(int index)
        {
            // You can also use "LastValue" property if you don't have index
            _lastValueTextBlock.Text = Source[index].ToString();

            // You can also use "Last(1)" property if you don't have index
            _lastClosedValueTextBlock.Text = Source[index - 1].ToString();

            _countTextBlock.Text = Source.Count.ToString();
        }
    }
}
