using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use the StretchDirection
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class StretchDirectionSample : Indicator
    {
        [Parameter("Stretch Direction", DefaultValue = StretchDirection.UpOnly)]
        public StretchDirection StretchDirection { get; set; }

        protected override void Initialize()
        {
            var image = new Image
            {
                Source = Properties.Resources.ctrader_logo,
                Width = 200,
                Height = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                StretchDirection = StretchDirection
            };

            Chart.AddControl(image);
        }

        public override void Calculate(int index)
        {
        }
    }
}