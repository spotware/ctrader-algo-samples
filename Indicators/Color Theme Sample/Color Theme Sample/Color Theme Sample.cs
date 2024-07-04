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
