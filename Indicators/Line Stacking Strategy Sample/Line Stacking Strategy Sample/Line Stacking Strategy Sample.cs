// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System.Text;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class LineStackingStrategySample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("First line of text");
            stringBuilder.AppendLine("Second line of text");
            stringBuilder.AppendLine("Third line of text");
            stringBuilder.AppendLine("Fourth line of text");
            stringBuilder.AppendLine("Fifth line of text");

            stackPanel.AddChild(new TextBlock
            {
                Margin = 5,
                Text = "LineStackingStrategy = BlockLineHeight:\n" + stringBuilder.ToString(),
                LineStackingStrategy = LineStackingStrategy.BlockLineHeight,
                FontWeight = FontWeight.Bold,
                ForegroundColor = Color.Black
            });

            stackPanel.AddChild(new TextBlock
            {
                Margin = 5,
                Text = "LineStackingStrategy = MaxHeight:\n" + stringBuilder.ToString(),
                LineStackingStrategy = LineStackingStrategy.MaxHeight,
                FontWeight = FontWeight.Bold,
                ForegroundColor = Color.Black
            });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
