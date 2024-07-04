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
    [Levels(30, 70)]
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class LevelsSample : Indicator
    {
        private RelativeStrengthIndex _rsi;

        [Parameter(DefaultValue = 0.0)]
        public double Parameter { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, 20);
        }

        public override void Calculate(int index)
        {
            Result[index] = _rsi.Result[index];
        }
    }
}
