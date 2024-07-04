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
    public class ImageSample : Indicator
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
            var image = new Image
            {
                Source = imageBytes,
                Width = 200,
                Height = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Chart.AddControl(image);
        }
        public override void Calculate(int index)
        {
        }
    }
}
