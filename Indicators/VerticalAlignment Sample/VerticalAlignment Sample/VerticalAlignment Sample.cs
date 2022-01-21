using cAlgo.API;

namespace cAlgo
{
    // This sample shows how to use the VerticalAlignment
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class VerticalAlignmentSample : Indicator
    {
        [Parameter("Vertical Alignment", DefaultValue = VerticalAlignment.Center)]
        public VerticalAlignment VerticalAlignment { get; set; }

        protected override void Initialize()
        {
            var textBlock = new TextBlock
            {
                Text = string.Format("Alignment: {0}", VerticalAlignment),
                VerticalAlignment = VerticalAlignment,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Chart.AddControl(textBlock);
        }

        public override void Calculate(int index)
        {
        }
    }
}