using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class AccumulativeSwingIndex : Indicator
    {
        private SwingIndex _swingIndexValues;

        [Parameter("Limit Move Value", DefaultValue = 12, MinValue = 0)]
        public int LimitMoveValue { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _swingIndexValues = Indicators.SwingIndex(LimitMoveValue);
        }

        public override void Calculate(int index)
        {
            Result[index] = index == 0 ? _swingIndexValues.Result[index] : _swingIndexValues.Result[index - 1] + _swingIndexValues.Result[index];
        }
    }
}