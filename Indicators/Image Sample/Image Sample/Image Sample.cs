using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use image control to show images
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ImageSample : Indicator
    {
        protected override void Initialize()
        {
            var image = new Image 
            {
                Source = Properties.Resources.Logo,
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
