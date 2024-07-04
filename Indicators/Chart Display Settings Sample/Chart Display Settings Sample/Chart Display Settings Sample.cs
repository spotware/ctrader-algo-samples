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
    public class ChartDisplaySettingsSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                BackgroundColor = Color.Gold,
                Opacity = 0.7,
                Margin = 5,
                Orientation = Orientation.Vertical
            };

            var askPriceLineCheckBox = new CheckBox
            {
                Text = "Ask Price Line",
                Margin = 5,
                IsChecked = Chart.DisplaySettings.AskPriceLine
            };

            askPriceLineCheckBox.Click += args => Chart.DisplaySettings.AskPriceLine = args.CheckBox.IsChecked.Value;

            stackPanel.AddChild(askPriceLineCheckBox);

            var bidPriceLineCheckBox = new CheckBox
            {
                Text = "Bid Price Line",
                Margin = 5,
                IsChecked = Chart.DisplaySettings.BidPriceLine
            };

            bidPriceLineCheckBox.Click += args => Chart.DisplaySettings.BidPriceLine = args.CheckBox.IsChecked.Value;

            stackPanel.AddChild(bidPriceLineCheckBox);

            var chartScaleCheckBox = new CheckBox
            {
                Text = "Chart Scale",
                Margin = 5,
                IsChecked = Chart.DisplaySettings.ChartScale
            };

            chartScaleCheckBox.Click += args => Chart.DisplaySettings.ChartScale = args.CheckBox.IsChecked.Value;

            stackPanel.AddChild(chartScaleCheckBox);

            var dealMapCheckBox = new CheckBox
            {
                Text = "Deal Map",
                Margin = 5,
                IsChecked = Chart.DisplaySettings.DealMap
            };

            dealMapCheckBox.Click += args => Chart.DisplaySettings.DealMap = args.CheckBox.IsChecked.Value;

            stackPanel.AddChild(dealMapCheckBox);

            var gridCheckBox = new CheckBox
            {
                Text = "Grid",
                Margin = 5,
                IsChecked = Chart.DisplaySettings.Grid
            };

            gridCheckBox.Click += args => Chart.DisplaySettings.Grid = args.CheckBox.IsChecked.Value;

            stackPanel.AddChild(gridCheckBox);

            var volumeCheckBox = new CheckBox
            {
                Text = "Volume",
                Margin = 5,
                IsChecked = Chart.DisplaySettings.TickVolume
            };

            volumeCheckBox.Click += args => Chart.DisplaySettings.TickVolume = args.CheckBox.IsChecked.Value;

            stackPanel.AddChild(volumeCheckBox);

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
