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
    public class FunctionsSample : Indicator
    {
        private SimpleMovingAverage _smaFast, _smaSlow;

        protected override void Initialize()
        {
            _smaFast = Indicators.SimpleMovingAverage(Bars.ClosePrices, 9);
            _smaSlow = Indicators.SimpleMovingAverage(Bars.ClosePrices, 20);
        }

        public override void Calculate(int index)
        {
            if (_smaFast.Result.HasCrossedAbove(_smaSlow.Result, 1))
            {
                // Fast MA crossed above slow MA
            }

            if (_smaFast.Result.HasCrossedBelow(_smaSlow.Result, 1))
            {
                // Fast MA crossed below slow MA
            }

            if (_smaFast.Result.Maximum(10) > _smaSlow.Result.Maximum(10))
            {
                // Fast MA last 10 values maximum is larger than slow MA last 10 values
            }

            if (_smaFast.Result.Minimum(10) < _smaSlow.Result.Minimum(10))
            {
                // Fast MA last 10 values minimum is smaller than slow MA last 10 values
            }

            if (_smaFast.Result.IsFalling() && _smaSlow.Result.IsRising())
            {
                // Fast MA is falling and slow MA is raising
                // IsFalling and IsRising compares last two values of the data series
            }

            if (_smaFast.Result.Sum(10) > _smaSlow.Result.Sum(10))
            {
                // Fast MA last 10 values sum is larger than slow MA last 109 values sum
            }
        }
    }
}
