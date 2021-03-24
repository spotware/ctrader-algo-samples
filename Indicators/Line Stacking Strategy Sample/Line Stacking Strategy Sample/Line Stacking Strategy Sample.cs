using cAlgo.API;
using System.Text;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use different Line Stacking Strategies on a TextBlock
    /// </summary>
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