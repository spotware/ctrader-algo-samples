using cAlgo.API;
using System.Text;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Chart.DrawStaticText method to draw static locked text on chart
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ChartStaticTextSample : Indicator
    {
        protected override void Initialize()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Symbol: " + SymbolName);
            stringBuilder.AppendLine("TimeFrame: " + TimeFrame);
            stringBuilder.AppendLine("Chart Type: " + Chart.ChartType);

            Chart.DrawStaticText("Static_Sample", stringBuilder.ToString(), VerticalAlignment.Bottom, HorizontalAlignment.Left, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}
