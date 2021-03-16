using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to parse a string to color and use it in your code
    /// You can use both Hex color codes and color names for Color Code parameter
    /// </summary>
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ColorSample : Indicator
    {
        [Parameter("Color Code", DefaultValue = "#168565")]
        public string ColorCodeParameter { get; set; }

        [Parameter("Color Alpha", DefaultValue = 100, MinValue = 0, MaxValue = 255)]
        public int ColorAlphaParameter { get; set; }

        protected override void Initialize()
        {
            Chart.ColorSettings.BackgroundColor = ParseColor(ColorCodeParameter, ColorAlphaParameter);
        }

        public override void Calculate(int index)
        {
        }

        private Color ParseColor(string colorString, int alpha = 255)
        {
            var color = colorString[0] == '#' ? Color.FromHex(colorString) : Color.FromName(colorString);

            return Color.FromArgb(alpha, color);
        }
    }
}
