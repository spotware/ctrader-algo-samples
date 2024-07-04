// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;
using System;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class OutputSnapshotSample : Indicator
    {
        private MarketSnapshotControl _marketSnapshotControl;

        private MovingAverage _ma;

        [Parameter("Tolerance (Pips)", DefaultValue = 8)]
        public double ToleranceInPips { get; set; }

        [Output("Main", LineColor = "Yellow", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Main { get; set; }

        protected override void Initialize()
        {
            ToleranceInPips *= Symbol.PipSize;

            _ma = Indicators.MovingAverage(Bars.ClosePrices, 9, MovingAverageType.Exponential);

            _marketSnapshotControl = new MarketSnapshotControl
            {
                BackgroundColor = Color.Gold,
                BorderColor = Color.Gray,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Opacity = 0.6,
                Margin = 2,
                IsVisible = false,
                Width = 220,
                Height = 50
            };

            IndicatorArea.AddControl(_marketSnapshotControl);

            IndicatorArea.MouseMove += IndicatorArea_MouseMove;
            IndicatorArea.MouseLeave += IndicatorArea_MouseLeave;
        }

        private void IndicatorArea_MouseMove(ChartMouseEventArgs obj)
        {
            var index = (int)obj.BarIndex;
            var yValue = Math.Round(obj.YValue, Symbol.Digits);
            var mainValue = Math.Round(Main[index], Symbol.Digits);

            if (double.IsNaN(mainValue) || Math.Abs(mainValue - yValue) > ToleranceInPips)
            {
                _marketSnapshotControl.IsVisible = false;

                return;
            }

            var extraDelta = 10;
            var width = _marketSnapshotControl.Width;
            var height = _marketSnapshotControl.Height;
            var left = Chart.Width - obj.MouseX > width + extraDelta ? obj.MouseX + extraDelta : obj.MouseX - width - extraDelta;
            var right = Chart.Height - obj.MouseY > height + extraDelta ? obj.MouseY + extraDelta : obj.MouseY - height - extraDelta;

            _marketSnapshotControl.Margin = new Thickness(left, right, 0, 0);

            _marketSnapshotControl.Time = Bars.OpenTimes[index].ToString("g");
            _marketSnapshotControl.Value = mainValue.ToString();

            _marketSnapshotControl.IsVisible = true;
        }

        private void IndicatorArea_MouseLeave(ChartMouseEventArgs obj)
        {
            _marketSnapshotControl.IsVisible = false;
        }

        public override void Calculate(int index)
        {
            Main[index] = _ma.Result[index];
        }
    }

    public class MarketSnapshotControl : CustomControl
    {
        private readonly Border _border;

        private TextBlock _valueTextBlock;

        private TextBlock _timeTextBlock;

        public MarketSnapshotControl()
        {
            _border = new Border
            {
                BackgroundColor = "#3F3F3F",
                BorderColor = "#969696",
                BorderThickness = 1,
                CornerRadius = 5
            };

            var style = new Style();

            style.Set(ControlProperty.Margin, new Thickness(3, 3, 0, 0));
            style.Set(ControlProperty.FontWeight, FontWeight.Bold);
            style.Set(ControlProperty.ForegroundColor, Color.Black);
            style.Set(ControlProperty.HorizontalContentAlignment, HorizontalAlignment.Left);
            style.Set(ControlProperty.VerticalContentAlignment, VerticalAlignment.Center);

            _valueTextBlock = new TextBlock
            {
                Style = style
            };
            _timeTextBlock = new TextBlock
            {
                Style = style
            };

            var grid = new Grid(2, 2)
            {
                ShowGridLines = true
            };

            grid.AddChild(new TextBlock
            {
                Text = "Value",
                Style = style
            }, 0, 0);
            grid.AddChild(new TextBlock
            {
                Text = "Time",
                Style = style
            }, 1, 0);

            grid.AddChild(_valueTextBlock, 0, 1);
            grid.AddChild(_timeTextBlock, 1, 1);

            _border.Child = grid;

            AddChild(_border);
        }

        public Color BackgroundColor
        {
            get { return _border.BackgroundColor; }
            set { _border.BackgroundColor = value; }
        }

        public Color BorderColor
        {
            get { return _border.BorderColor; }
            set { _border.BorderColor = value; }
        }

        public string Value
        {
            get { return _valueTextBlock.Text; }
            set { _valueTextBlock.Text = value; }
        }

        public string Time
        {
            get { return _timeTextBlock.Text; }
            set { _timeTextBlock.Text = value; }
        }
    }
}
