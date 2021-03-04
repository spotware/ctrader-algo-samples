using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CanvasSample : Indicator
    {
        protected override void Initialize()
        {
            var canvas = new Canvas
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 300,
                Height = 200,
                BackgroundColor = Color.Red,
                Opacity = 0.5
            };

            canvas.AddChild(new Button
            {
                Top = 20,
                Left = 80,
                Margin = 5,
                Text = "Button Inside Canvas"
            });

            canvas.AddChild(new Image
            {
                Source = Properties.Resources.stock,
                Margin = 5,
                Width = 128,
                Height = 128,
                Top = 45,
                Left = 80
            });

            Chart.AddControl(canvas);
        }

        public override void Calculate(int index)
        {
        }
    }
}