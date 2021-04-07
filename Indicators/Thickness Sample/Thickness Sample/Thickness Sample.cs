using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Thickness for defining a chart control margin
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ThicknessSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.6
            };

            var rectangle = new Rectangle
            {
                StrokeColor = Color.Blue,
                FillColor = Color.Red,
                StrokeThickness = 2,
                Margin = new Thickness(10, 5, 10, 5),
                Width = 300,
                Height = 100,
            };

            stackPanel.AddChild(rectangle);

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}