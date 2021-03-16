using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This is a sample of how to catch or handle the keyboard events on chart
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartKeyboardSample : Indicator
    {
        private TextBlock _keyDownTextBlock, _keyCombinationTextBlock;

        protected override void Initialize()
        {
            var stackPanel = new StackPanel 
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.7,
                Width = 200
            };

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Keyboard Handler",
                FontWeight = FontWeight.ExtraBold,
                Margin = 5,
                HorizontalAlignment = HorizontalAlignment.Center,
                ForegroundColor = Color.Black
            });

            var grid = new Grid(2, 2);

            grid.AddChild(new TextBlock 
            {
                Text = "Key Down",
                Margin = 5,
                ForegroundColor = Color.Black
            }, 0, 0);

            _keyDownTextBlock = new TextBlock 
            {
                Margin = 5,
                ForegroundColor = Color.Black
            };

            grid.AddChild(_keyDownTextBlock, 0, 1);

            grid.AddChild(new TextBlock 
            {
                Text = "Key Combination",
                Margin = 5,
                ForegroundColor = Color.Black
            }, 1, 0);

            _keyCombinationTextBlock = new TextBlock 
            {
                Margin = 5,
                ForegroundColor = Color.Black
            };

            grid.AddChild(_keyCombinationTextBlock, 1, 1);

            stackPanel.AddChild(grid);

            Chart.AddControl(stackPanel);

            Chart.KeyDown += Chart_KeyDown;
        }

        private void Chart_KeyDown(ChartKeyboardEventArgs obj)
        {
            _keyDownTextBlock.Text = obj.Key.ToString();

            _keyCombinationTextBlock.Text = string.Empty;

            if (obj.AltKey)
                _keyCombinationTextBlock.Text += "Alt, ";
            if (obj.ShiftKey)
                _keyCombinationTextBlock.Text += "Shift, ";
            if (obj.CtrlKey)
                _keyCombinationTextBlock.Text += "Ctrl, ";
        }

        public override void Calculate(int index)
        {
        }
    }
}
