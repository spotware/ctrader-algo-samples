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
    public class ChartSample : Indicator
    {
        private Grid _grid;

        private TextBlock _mouseLocationTextBlock;

        private TextBlock _mouseWheelDeltaTextBlock;

        private TextBlock _objectsNumberTextBlock;

        protected override void Initialize()
        {
            Chart.ChartTypeChanged += args => CreateAndAddGridToChart();
            Chart.ColorsChanged += args => CreateAndAddGridToChart();
            Chart.DisplaySettingsChanged += args => CreateAndAddGridToChart();
            Chart.Drag += args => CreateAndAddGridToChart();
            Chart.DragEnd += args => CreateAndAddGridToChart();
            Chart.DragStart += args => CreateAndAddGridToChart();
            Chart.IndicatorAreaAdded += args => CreateAndAddGridToChart();
            Chart.IndicatorAreaRemoved += args => CreateAndAddGridToChart();
            Chart.MouseMove += args =>
            {
                if (_mouseLocationTextBlock == null)
                    return;

                _mouseLocationTextBlock.Text = string.Format("({0}, {1})", args.MouseX, args.MouseY);
            };
            Chart.MouseLeave += args =>
            {
                if (_mouseLocationTextBlock == null)
                    return;

                _mouseLocationTextBlock.Text = "(Null, Null)";
                _mouseWheelDeltaTextBlock.Text = "0";
            };
            Chart.MouseWheel += args =>
            {
                if (_mouseWheelDeltaTextBlock == null)
                    return;

                _mouseWheelDeltaTextBlock.Text = args.Delta.ToString();
            };
            Chart.ObjectsAdded += args => _objectsNumberTextBlock.Text = Chart.Objects.Count.ToString();
            Chart.ObjectsRemoved += args => _objectsNumberTextBlock.Text = Chart.Objects.Count.ToString();

            Chart.ZoomChanged += args => CreateAndAddGridToChart();

            CreateAndAddGridToChart();
        }

        public override void Calculate(int index)
        {
        }

        private void CreateAndAddGridToChart()
        {
            if (_grid != null)
                Chart.RemoveControl(_grid);

            _grid = new Grid(10, 2)
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            var style = new Style();

            style.Set(ControlProperty.Margin, 5);
            style.Set(ControlProperty.FontWeight, FontWeight.ExtraBold);
            style.Set(ControlProperty.ForegroundColor, Color.Red);

            _grid.AddChild(new TextBlock
            {
                Text = "Height",
                Style = style
            }, 0, 0);
            _grid.AddChild(new TextBlock
            {
                Text = Chart.Height.ToString(),
                Style = style
            }, 0, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Width",
                Style = style
            }, 1, 0);
            _grid.AddChild(new TextBlock
            {
                Text = Chart.Width.ToString(),
                Style = style
            }, 1, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Zoom Level",
                Style = style
            }, 2, 0);
            _grid.AddChild(new TextBlock
            {
                Text = Chart.ZoomLevel.ToString(),
                Style = style
            }, 2, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Objects #",
                Style = style
            }, 3, 0);

            _objectsNumberTextBlock = new TextBlock
            {
                Style = style,
                Text = Chart.Objects.Count.ToString()
            };

            _grid.AddChild(_objectsNumberTextBlock, 3, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Top Y",
                Style = style
            }, 4, 0);
            _grid.AddChild(new TextBlock
            {
                Text = Chart.TopY.ToString(),
                Style = style
            }, 4, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Bottom Y",
                Style = style
            }, 5, 0);
            _grid.AddChild(new TextBlock
            {
                Text = Chart.BottomY.ToString(),
                Style = style
            }, 5, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Type",
                Style = style
            }, 6, 0);
            _grid.AddChild(new TextBlock
            {
                Text = Chart.ChartType.ToString(),
                Style = style
            }, 6, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Mouse Location",
                Style = style
            }, 7, 0);

            _mouseLocationTextBlock = new TextBlock
            {
                Style = style,
                Text = "(Null, Null)"
            };

            _grid.AddChild(_mouseLocationTextBlock, 7, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Indicator Areas #",
                Style = style
            }, 8, 0);
            _grid.AddChild(new TextBlock
            {
                Text = Chart.IndicatorAreas.Count.ToString(),
                Style = style
            }, 8, 1);

            _grid.AddChild(new TextBlock
            {
                Text = "Mouse Wheel Delta",
                Style = style
            }, 9, 0);

            _mouseWheelDeltaTextBlock = new TextBlock
            {
                Style = style,
                Text = "0"
            };

            _grid.AddChild(_mouseWheelDeltaTextBlock, 9, 1);

            Chart.AddControl(_grid);
        }
    }
}
