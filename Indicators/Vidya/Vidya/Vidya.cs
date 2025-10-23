using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AutoRescale = false, AccessRights = AccessRights.None)]
    public class Vidya : Indicator
    {
        [Parameter]
        public DataSeries Price { get; set; }

        [Parameter(DefaultValue = 14, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Sigma", DefaultValue = 0.65, MinValue = 0.1, MaxValue = 0.95)]
        public double Sigma { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            var prevResult = Result[index - 1];

            if (double.IsNaN(prevResult))
                prevResult = Price[index - 1];

            var k = Sigma * Cmo(index);
            Result[index] = (1 - k) * prevResult + k * Price[index];
        }

        private double Cmo(int index)
        {
            double sumUp = 0;
            double sumDown = 0;

            for (var i = 0; i < Periods; i++)
            {
                var difference = Price[index - i] - Price[index - i - 1];

                if (difference > 0)
                    sumUp += difference;
                else
                    sumDown += -difference;
            }

            if (sumUp + sumDown == 0)
                return 0.0;

            return Math.Abs((sumUp - sumDown) / (sumUp + sumDown));
        }
    }
}