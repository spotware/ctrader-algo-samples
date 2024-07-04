// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class BarsSample : Indicator
    {
        private TextBlock _barTicksNumberTextBlock, _barsStateTextBlock;

        [Output("Range", LineColor = "RoyalBlue")]
        public IndicatorDataSeries Range { get; set; }

        [Output("Body", LineColor = "Yellow")]
        public IndicatorDataSeries Body { get; set; }

        protected override void Initialize()
        {
            Bars.BarOpened += Bars_BarOpened;

            Bars.Tick += Bars_Tick;

            Bars.HistoryLoaded += Bars_HistoryLoaded;

            Bars.Reloaded += Bars_Reloaded;

            var grid = new Grid(2, 2)
            {
                BackgroundColor = Color.DarkGoldenrod,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Opacity = 0.5
            };

            grid.AddChild(new TextBlock
            {
                Text = "Bar Ticks #",
                Margin = 5
            }, 0, 0);

            _barTicksNumberTextBlock = new TextBlock
            {
                Text = "0",
                Margin = 5
            };

            grid.AddChild(_barTicksNumberTextBlock, 0, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Bars State",
                Margin = 5
            }, 1, 0);

            _barsStateTextBlock = new TextBlock
            {
                Margin = 5
            };

            grid.AddChild(_barsStateTextBlock, 1, 1);

            IndicatorArea.AddControl(grid);
        }

        private void Bars_Reloaded(BarsHistoryLoadedEventArgs obj)
        {
            _barsStateTextBlock.Text = "Reloaded";
        }

        private void Bars_HistoryLoaded(BarsHistoryLoadedEventArgs obj)
        {
            _barsStateTextBlock.Text = "History Loaded";
        }

        private void Bars_Tick(BarsTickEventArgs obj)
        {
            _barTicksNumberTextBlock.Text = Bars.TickVolumes.LastValue.ToString();
        }

        private void Bars_BarOpened(BarOpenedEventArgs obj)
        {
            _barsStateTextBlock.Text = "New Bar Opened";
        }

        public override void Calculate(int index)
        {
            Range[index] = Bars.HighPrices[index] - Bars.LowPrices[index];
            Body[index] = Math.Abs(Bars.ClosePrices[index] - Bars.OpenPrices[index]);
        }
    }
}
