// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
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
