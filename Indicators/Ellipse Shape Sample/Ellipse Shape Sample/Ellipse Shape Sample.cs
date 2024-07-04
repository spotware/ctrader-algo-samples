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
    public class EllipseShapeSample : Indicator
    {
        protected override void Initialize()
        {
            var ellipse = new Ellipse
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = 5,
                Width = 100,
                Height = 200,
                StrokeColor = Color.Black,
                FillColor = Color.Aqua,
                StrokeThickness = 2,
                StrokeStartLineCap = PenLineCap.Square,
                Left = 100,
                Top = 50
            };

            var canvas = new Canvas
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.5
            };

            canvas.AddChild(ellipse);

            Chart.AddControl(canvas);
        }

        public override void Calculate(int index)
        {
        }
    }
}
