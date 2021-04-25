using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use the rectangle corner radius to create a round corner rectangle
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RectangleCornerRadiusSample : Indicator
    {
        protected override void Initialize()
        {
            var rectangle = new Rectangle 
            {
                RadiusX = 20,
                RadiusY = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 200,
                Height = 150,
                FillColor = Color.FromArgb(100, Color.Red),
                StrokeColor = Color.Yellow
            };

            Chart.AddControl(rectangle);
        }

        public override void Calculate(int index)
        {
        }
    }
}
