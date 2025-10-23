using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class ChandeMomentumOscillator : Indicator
    {
        private IndicatorDataSeries _difference;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 20, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            Source ??= Bars.ClosePrices;
            _difference = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            var outputIndex = index + Shift;

            _difference[index] = index > 0 ? Source[index] - Source[index - 1] : double.NaN;
            if (index < Periods)
            {
                Result[outputIndex] = double.NaN;
                return;
            }

            double sumPositive = 0;
            double sumNegative = 0;
            for (var i = 0; i < Periods; i++)
            {
                var difference = _difference[index - i];
                if (difference > 0)
                    sumPositive += difference;
                else
                    sumNegative += Math.Abs(difference);
            }

            Result[outputIndex] = sumPositive + sumNegative == 0 ? double.NaN : (sumPositive - sumNegative) / (sumPositive + sumNegative) * 100;
        }
    }
}