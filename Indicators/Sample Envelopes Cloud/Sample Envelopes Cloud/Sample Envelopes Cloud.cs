// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Cloud(UpperBandLineName, LowerBandLineName)]
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleEnvelopesCloud : Indicator
    {
        private const string UpperBandLineName = "UpperBand";
        private const string LowerBandLineName = "LowerBand";

        [Parameter(DefaultValue = 14)]
        public int Period { get; set; }

        [Parameter(DefaultValue = 0.1)]
        public double Deviation { get; set; }

        [Output(UpperBandLineName, LineColor = "#B268BCFF")]
        public IndicatorDataSeries UpperBand { get; set; }

        [Output(LowerBandLineName, LineColor = "#B2FF5861")]
        public IndicatorDataSeries LowerBand { get; set; }

        private MovingAverage _movingAverage;

        protected override void Initialize()
        {
            _movingAverage = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            var maValue = _movingAverage.Result[index];
            UpperBand[index] = maValue * (1 + Deviation / 100);
            LowerBand[index] = maValue * (1 - Deviation / 100);
        }
    }
}
