using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Polyline to draw connected lines
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PolylineSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.AddControl(new Polyline
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeColor = Color.Red,
                StrokeThickness = 1,
                Points = new[]
                {
                    new Point(10, 10),
                    new Point(100,200),
                    new Point(10, 100),
                    new Point(10, 10),
                }
            });
        }

        public override void Calculate(int index)
        {
        }
    }
}