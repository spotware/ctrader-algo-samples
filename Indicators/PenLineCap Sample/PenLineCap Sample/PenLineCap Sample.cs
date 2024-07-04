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
    public class PenLineCapSample : Indicator
    {
        [Parameter("Stroke Start Line Cap", DefaultValue = PenLineCap.Flat)]
        public PenLineCap StrokeStartLineCap { get; set; }

        [Parameter("Stroke End Line Cap", DefaultValue = PenLineCap.Flat)]
        public PenLineCap StrokeEndLineCap { get; set; }

        [Parameter("Stroke Dash Cap", DefaultValue = PenLineCap.Flat)]
        public PenLineCap StrokeDashCap { get; set; }

        protected override void Initialize()
        {
            var rectangle = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeStartLineCap = StrokeStartLineCap,
                StrokeEndLineCap = StrokeEndLineCap,
                StrokeDashCap = StrokeDashCap,
                StrokeColor = Color.Red,
                StrokeThickness = 4,
                Width = 200,
                Height = 100
            };

            Chart.AddControl(rectangle);
        }

        public override void Calculate(int index)
        {
        }
    }
}
