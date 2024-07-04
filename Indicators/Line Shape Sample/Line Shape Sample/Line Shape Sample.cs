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
    public class LineShapeSample : Indicator
    {
        protected override void Initialize()
        {
            var xCenter = Chart.Width / 2;
            var yCenter = Chart.Height / 2;

            var line = new Line
            {
                X1 = xCenter,
                X2 = xCenter + 100,
                Y1 = yCenter,
                Y2 = yCenter + 100,
                StrokeColor = Color.Red,
                StrokeThickness = 2
            };

            Chart.AddControl(line);
        }

        public override void Calculate(int index)
        {
        }
    }
}
