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
    public class ApplicationSample : Indicator
    {
        private TextBlock _userTimeOffsetTextBlock, _themeTextBlock;

        [Parameter("Horizontal Alignment", DefaultValue = HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [Parameter("Vertical Alignment", DefaultValue = VerticalAlignment.Center)]
        public VerticalAlignment VerticalAlignment { get; set; }

        protected override void Initialize()
        {
            Application.ColorThemeChanged += Application_ColorThemeChanged;
            Application.UserTimeOffsetChanged += Application_UserTimeOffsetChanged;

            DrawApplicationInfo();
        }

        private void Application_UserTimeOffsetChanged(UserTimeOffsetChangedEventArgs obj)
        {
            _userTimeOffsetTextBlock.Text = obj.UserTimeOffset.ToString();
        }

        private void Application_ColorThemeChanged(ColorThemeChangeEventArgs obj)
        {
            _themeTextBlock.Text = obj.ColorTheme.ToString();
        }

        private void DrawApplicationInfo()
        {
            var grid = new Grid(3, 2)
            {
                BackgroundColor = Color.Goldenrod,
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment
            };

            grid.AddChild(new TextBlock
            {
                Text = "Version",
                Margin = 5
            }, 0, 0);
            grid.AddChild(new TextBlock
            {
                Text = Application.Version.ToString(),
                Margin = 5
            }, 0, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Theme",
                Margin = 5
            }, 1, 0);

            _themeTextBlock = new TextBlock
            {
                Text = Application.ColorTheme.ToString(),
                Margin = 5
            };

            grid.AddChild(_themeTextBlock, 1, 1);

            grid.AddChild(new TextBlock
            {
                Text = "User Time Offset",
                Margin = 5
            }, 2, 0);

            _userTimeOffsetTextBlock = new TextBlock
            {
                Text = Application.UserTimeOffset.ToString(),
                Margin = 5
            };

            grid.AddChild(_userTimeOffsetTextBlock, 2, 1);

            Chart.AddControl(grid);
        }

        public override void Calculate(int index)
        {
        }
    }
}
