using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class CustomData : Indicator
    {
        [Parameter(DefaultValue = 0.0)]
        public double Parameter { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        private Tooltip _tooltip;

        protected override void Initialize()
        {
            var content = new TextBlock { Text = "Here you can put any control", Margin = 5 };

            _tooltip = new Tooltip(content)
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsVisible = false,
                Width = 160,
                Height = 40
            };

            Chart.AddControl(_tooltip);

            Chart.MouseMove += OnMouseMove;
            Chart.MouseLeave += OnMouseLeave;
            Chart.MouseEnter += Chart_MouseEnter;
        }

        private void Chart_MouseEnter(ChartMouseEventArgs obj)
        {
            _tooltip.IsVisible = true;
        }

        public override void Calculate(int index)
        {
            // Calculate value at specified index
            // Result[index] = ...
        }

        protected void OnMouseMove(ChartMouseEventArgs obj)
        {
            var extraDelta = 10;
            var width = _tooltip.Width;
            var height = _tooltip.Height;
            var left = Chart.Width - obj.MouseX > width + extraDelta ? obj.MouseX + extraDelta : obj.MouseX - width - extraDelta;
            var right = Chart.Height - obj.MouseY > height + extraDelta ? obj.MouseY + extraDelta : obj.MouseY - height - extraDelta;

            _tooltip.Margin = new Thickness(left, right, 0, 0);
        }

        protected void OnMouseLeave(ChartMouseEventArgs obj)
        {
            _tooltip.IsVisible = false;
        }
    }

    public class Tooltip : CustomControl
    {
        public Tooltip(ControlBase content)
        {
            var border = new Border
            {
                BackgroundColor = "#3F3F3F",
                BorderColor = "#969696",
                BorderThickness = 1,
                CornerRadius = 5
            };

            border.Child = content;

            AddChild(border);
        }
    }
}