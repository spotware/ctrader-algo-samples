using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to draw an ellipse shape on your chart
    /// </summary>
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
