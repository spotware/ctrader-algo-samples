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
    public class MarketSnapshotSample : Indicator
    {
        private MarketSnapshotControl _marketSnapshotControl;

        protected override void Initialize()
        {
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
                Height = 200
            };

            Chart.AddControl(_marketSnapshotControl);

            Chart.MouseDown += Chart_MouseDown;
            Chart.MouseUp += Chart_MouseUp;
        }

        private void Chart_MouseDown(ChartMouseEventArgs obj)
        {
            var extraDelta = 10;
            var width = _marketSnapshotControl.Width;
            var height = _marketSnapshotControl.Height;
            var left = Chart.Width - obj.MouseX > width + extraDelta ? obj.MouseX + extraDelta : obj.MouseX - width - extraDelta;
            var right = Chart.Height - obj.MouseY > height + extraDelta ? obj.MouseY + extraDelta : obj.MouseY - height - extraDelta;

            _marketSnapshotControl.Margin = new Thickness(left, right, 0, 0);

            var index = (int)obj.BarIndex;

            _marketSnapshotControl.Open = Bars.OpenPrices[index].ToString();
            _marketSnapshotControl.High = Bars.HighPrices[index].ToString();
            _marketSnapshotControl.Low = Bars.LowPrices[index].ToString();
            _marketSnapshotControl.Close = Bars.ClosePrices[index].ToString();
            _marketSnapshotControl.Time = Bars.OpenTimes[index].ToString("g");
            _marketSnapshotControl.Volume = Bars.TickVolumes[index].ToString();

            _marketSnapshotControl.IsVisible = true;
        }

        private void Chart_MouseUp(ChartMouseEventArgs obj)
        {
            _marketSnapshotControl.IsVisible = false;
        }

        public override void Calculate(int index)
        {
        }
    }

    public class MarketSnapshotControl : CustomControl
    {
        private readonly Border _border;

        private TextBlock _openTextBlock;

        private TextBlock _highTextBlock;

        private TextBlock _lowTextBlock;

        private TextBlock _closeTextBlock;

        private TextBlock _volumeTextBlock;

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

            _openTextBlock = new TextBlock
            {
                Style = style
            };
            _highTextBlock = new TextBlock
            {
                Style = style
            };
            _lowTextBlock = new TextBlock
            {
                Style = style
            };
            _closeTextBlock = new TextBlock
            {
                Style = style
            };
            _volumeTextBlock = new TextBlock
            {
                Style = style
            };
            _timeTextBlock = new TextBlock
            {
                Style = style
            };

            var grid = new Grid(6, 2)
            {
                ShowGridLines = true
            };

            grid.AddChild(new TextBlock
            {
                Text = "Open",
                Style = style
            }, 0, 0);
            grid.AddChild(new TextBlock
            {
                Text = "High",
                Style = style
            }, 1, 0);
            grid.AddChild(new TextBlock
            {
                Text = "Low",
                Style = style
            }, 2, 0);
            grid.AddChild(new TextBlock
            {
                Text = "Close",
                Style = style
            }, 3, 0);
            grid.AddChild(new TextBlock
            {
                Text = "Volume",
                Style = style
            }, 4, 0);
            grid.AddChild(new TextBlock
            {
                Text = "Time",
                Style = style
            }, 5, 0);

            grid.AddChild(_openTextBlock, 0, 1);
            grid.AddChild(_highTextBlock, 1, 1);
            grid.AddChild(_lowTextBlock, 2, 1);
            grid.AddChild(_closeTextBlock, 3, 1);
            grid.AddChild(_volumeTextBlock, 4, 1);
            grid.AddChild(_timeTextBlock, 5, 1);

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

        public string Open
        {
            get { return _openTextBlock.Text; }
            set { _openTextBlock.Text = value; }
        }

        public string High
        {
            get { return _highTextBlock.Text; }
            set { _highTextBlock.Text = value; }
        }

        public string Low
        {
            get { return _lowTextBlock.Text; }
            set { _lowTextBlock.Text = value; }
        }

        public string Close
        {
            get { return _closeTextBlock.Text; }
            set { _closeTextBlock.Text = value; }
        }

        public string Volume
        {
            get { return _volumeTextBlock.Text; }
            set { _volumeTextBlock.Text = value; }
        }

        public string Time
        {
            get { return _timeTextBlock.Text; }
            set { _timeTextBlock.Text = value; }
        }
    }
}
