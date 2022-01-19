using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use ControlProperty in control style
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ControlPropertySample : Indicator
    {
        protected override void Initialize()
        {
            var style = new Style();

            style.Set(ControlProperty.Margin, 5);
            style.Set(ControlProperty.ForegroundColor, Color.Blue);
            style.Set(ControlProperty.FontSize, 14);
            style.Set(ControlProperty.Width, 100);

            var textBlock = new TextBlock
            {
                Text = "Styled Text Block",
                Style = style,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Chart.AddControl(textBlock);
        }

        public override void Calculate(int index)
        {
        }
    }
}