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
