using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use the HorizontalAlignment
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class HorizontalAlignmentSample : Indicator
    {
        [Parameter("Horizontal Alignment", DefaultValue = HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        protected override void Initialize()
        {
            var textBlock = new TextBlock
            {
                Text = string.Format("Alignment: {0}", HorizontalAlignment),
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment.Center
            };

            Chart.AddControl(textBlock);
        }

        public override void Calculate(int index)
        {
        }
    }
}