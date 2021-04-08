using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use the chart controls Horizontal/Vertical Alignment to align your controls
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ControlAlignmentSample : Indicator
    {
        [Parameter("Horizontal Alignment", DefaultValue = HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [Parameter("Vertical Alignment", DefaultValue = VerticalAlignment.Center)]
        public VerticalAlignment VerticalAlignment { get; set; }

        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment,
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                Width = 200,
                Height = 100
            };

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}