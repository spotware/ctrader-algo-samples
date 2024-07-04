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
