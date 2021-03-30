using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use a shape Stroke Line Join
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PenLineJoinSample : Indicator
    {
        [Parameter("Stroke Line Join", DefaultValue = PenLineJoin.Miter)]
        public PenLineJoin StrokeLineJoin { get; set; }

        protected override void Initialize()
        {
            var rectangle = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeLineJoin = StrokeLineJoin,
                StrokeColor = Color.Red,
                StrokeThickness = 4,
                Width = 200,
                Height = 100,
            };

            Chart.AddControl(rectangle);
        }

        public override void Calculate(int index)
        {
        }
    }
}