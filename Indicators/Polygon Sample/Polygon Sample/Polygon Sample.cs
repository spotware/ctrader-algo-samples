using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to draw a Polygon
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PolygonSample : Indicator
    {
        protected override void Initialize()
        {
            Chart.AddControl(new Polygon
            {
                FillColor = Color.Red,
                Width = 200,
                Height = 100,
                Margin = 10,
                Points = new Point[]
                {
                    new Point(100, 100),
                    new Point(200, 50),
                    new Point(300, 100),
                    new Point(100, 100),
                }
            });
        }

        public override void Calculate(int index)
        {
        }
    }
}