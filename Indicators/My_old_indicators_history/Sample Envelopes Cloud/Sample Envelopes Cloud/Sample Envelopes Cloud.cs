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
