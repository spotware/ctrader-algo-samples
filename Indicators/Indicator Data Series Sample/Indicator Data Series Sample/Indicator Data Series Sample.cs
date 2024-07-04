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
