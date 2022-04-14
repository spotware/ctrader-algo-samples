using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use an indicator data series
    /// </summary>
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class IndicatorDataSeriesSample : Indicator
    {
        private IndicatorDataSeries _internalSeries;

        [Output("Main", LineColor = "Yellow", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Main { get; set; }

        protected override void Initialize()
        {
            // If an indicator data series doesn't has the Output attribute then you must create it manually
            _internalSeries = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            _internalSeries[index] = Bars.HighPrices[index];

            Main[index] = _internalSeries[index] - Bars.LowPrices[index];
        }
    }
}
