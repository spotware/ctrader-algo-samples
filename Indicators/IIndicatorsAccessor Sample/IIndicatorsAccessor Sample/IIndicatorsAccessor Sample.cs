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
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class IIndicatorsAccessorSample : Indicator
    {
        private SimpleMovingAverage _sma;

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _sma = Indicators.SimpleMovingAverage(Bars.ClosePrices, 20);
        }

        public override void Calculate(int index)
        {
            Result[index] = _sma.Result[index];
        }
    }
}
