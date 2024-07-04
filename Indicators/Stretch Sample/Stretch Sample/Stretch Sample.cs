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
    public class StretchSample : Indicator
    {
        [Parameter("Stretch", DefaultValue = Stretch.Uniform)]
        public Stretch Stretch { get; set; }

        protected override void Initialize()
        {
            var rectangle = new Rectangle
            {
                Stretch = Stretch,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 100,
                Width = 200,
                FillColor = Color.Blue,
                StrokeColor = Color.Red,
                Opacity = 0.7
            };

            Chart.AddControl(rectangle);
        }

        public override void Calculate(int index)
        {
        }
    }
}
