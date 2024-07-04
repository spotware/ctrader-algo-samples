// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PlotTypeSample : Indicator
    {
        private StandardDeviation _standardDeviation;

        [Output("Discontinuous Line", LineColor = "Red", PlotType = PlotType.DiscontinuousLine)]
        public IndicatorDataSeries DiscontinuousLine { get; set; }

        [Output("Histogram", LineColor = "Green", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Histogram { get; set; }

        [Output("Line", LineColor = "Blue", PlotType = PlotType.Line)]
        public IndicatorDataSeries Line { get; set; }

        [Output("Points", LineColor = "Yellow", PlotType = PlotType.Points)]
        public IndicatorDataSeries Points { get; set; }

        protected override void Initialize()
        {
            _standardDeviation = Indicators.StandardDeviation(Bars.ClosePrices, 20, MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            DiscontinuousLine[index] = Bars.ClosePrices[index] + _standardDeviation.Result[index];
            Histogram[index] = Bars.ClosePrices[index] + (_standardDeviation.Result[index] * 1.5);
            Line[index] = Bars.ClosePrices[index] + (_standardDeviation.Result[index] * 2);
            Points[index] = Bars.ClosePrices[index] + (_standardDeviation.Result[index] * 2.5);
        }
    }
}
