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
                    new Point(100, 100)
                }
            });
        }

        public override void Calculate(int index)
        {
        }
    }
}
