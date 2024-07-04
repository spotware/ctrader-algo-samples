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
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class IndicatorAreaSample : Indicator
    {
        private TextBlock _indicatorAreaNumberTextBlock;

        protected override void Initialize()
        {
            var grid = new Grid(1, 2)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.7,
                Width = 200
            };

            grid.AddChild(new TextBlock
            {
                Text = "Indicator Area #",
                Margin = 5,
                FontWeight = FontWeight.ExtraBold,
                ForegroundColor = Color.Black
            }, 0, 0);

            _indicatorAreaNumberTextBlock = new TextBlock
            {
                Margin = 5,
                Text = Chart.IndicatorAreas.Count.ToString(),
                FontWeight = FontWeight.ExtraBold,
                ForegroundColor = Color.Black
            };

            grid.AddChild(_indicatorAreaNumberTextBlock, 0, 1);

            IndicatorArea.AddControl(grid);

            Chart.IndicatorAreaAdded += Chart_IndicatorAreaAdded;
            Chart.IndicatorAreaRemoved += Chart_IndicatorAreaRemoved;
        }

        private void Chart_IndicatorAreaRemoved(IndicatorAreaRemovedEventArgs obj)
        {
            _indicatorAreaNumberTextBlock.Text = Chart.IndicatorAreas.Count.ToString();
        }

        private void Chart_IndicatorAreaAdded(IndicatorAreaAddedEventArgs obj)
        {
            _indicatorAreaNumberTextBlock.Text = Chart.IndicatorAreas.Count.ToString();
        }

        public override void Calculate(int index)
        {
        }
    }
}
