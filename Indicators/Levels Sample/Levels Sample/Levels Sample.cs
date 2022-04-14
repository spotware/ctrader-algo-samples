using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to use the LevelsAttribute to set levels on your indicator outputs
    /// </summary>
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
