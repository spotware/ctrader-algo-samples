using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Chart ColorThemeChangeEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ColorThemeChangeEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Application.ColorThemeChanged += Application_ColorThemeChanged;
        }

        private void Application_ColorThemeChanged(ColorThemeChangeEventArgs obj)
        {
            var text = string.Format("Theme Changed To: {0}", obj.ColorTheme);

            Chart.DrawStaticText("text", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
