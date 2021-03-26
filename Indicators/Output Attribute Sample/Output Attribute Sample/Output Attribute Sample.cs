using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use indicators OuputAttribute
    /// </summary>
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class OutputAttributeSample : Indicator
    {
        [Output("Open", LineColor = "Red", IsHistogram = false, LineStyle = LineStyle.Dots, PlotType = PlotType.Line, Thickness = 2)]
        public IndicatorDataSeries OpenOutput { get; set; }

        [Output("High", LineColor = "Blue", IsHistogram = false, LineStyle = LineStyle.Solid, PlotType = PlotType.Line, Thickness = 2)]
        public IndicatorDataSeries HighOutput { get; set; }

        [Output("Low", LineColor = "Yellow", IsHistogram = false, LineStyle = LineStyle.Lines, PlotType = PlotType.Line, Thickness = 2)]
        public IndicatorDataSeries LowOutput { get; set; }

        [Output("Close", LineColor = "Green", IsHistogram = false, LineStyle = LineStyle.DotsRare, PlotType = PlotType.Line, Thickness = 2)]
        public IndicatorDataSeries CloseOutput { get; set; }

        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        {
            OpenOutput[index] = Bars.OpenPrices[index];
            HighOutput[index] = Bars.HighPrices[index];
            LowOutput[index] = Bars.LowPrices[index];
            CloseOutput[index] = Bars.ClosePrices[index];
        }
    }
}