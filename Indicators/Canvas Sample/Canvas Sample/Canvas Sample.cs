// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System.IO;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class CanvasSample : Indicator
    {
        [Parameter("Image File Path")]
        public string ImageFilePath { get; set; }

        protected override void Initialize()
        {

            if (!File.Exists(ImageFilePath))
            {
                Print($"Image not found: {ImageFilePath}");
                return;
            }
            var imageBytes = File.ReadAllBytes(ImageFilePath);

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
                Source = imageBytes,
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
