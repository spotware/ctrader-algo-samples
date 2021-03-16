using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use ColorTheme Enum which is the color of platform theme on your indicators/cBots
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ColorThemeSample : Indicator
    {
        protected override void Initialize()
        {
            ChangeChartBackgroundColorBasedOnTheme(Application.ColorTheme);

            Application.ColorThemeChanged += Application_ColorThemeChanged;
        }

        private void Application_ColorThemeChanged(ColorThemeChangeEventArgs obj)
        {
            ChangeChartBackgroundColorBasedOnTheme(obj.ColorTheme);
        }

        private void ChangeChartBackgroundColorBasedOnTheme(ColorTheme colorTheme)
        {
            if (colorTheme == ColorTheme.Dark)
            {
                Chart.ColorSettings.BackgroundColor = Color.White;
            }
            else if (colorTheme == ColorTheme.Light)
            {
                Chart.ColorSettings.BackgroundColor = Color.Black;
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}
