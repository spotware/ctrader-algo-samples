using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class MoneyFlowIndex : Indicator
    {
        [Parameter(DefaultValue = 14, MinValue = 2)]
        public int Periods { get; set; }

        [Parameter(DefaultValue = 0, MinValue = -1000, MaxValue = 1000)]
        public int Shift { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        public override void Calculate(int index)
        {
            if (index < Periods)
                return;

            var positiveMoneyFlow = CalculateMoneyFlow(index, (current, previous) => current > previous);
            var negativeMoneyFlow = CalculateMoneyFlow(index, (current, previous) => current < previous);

            var moneyFlowRatio = positiveMoneyFlow / negativeMoneyFlow;

            Result[index + Shift] = 100 - 100 / (1 + moneyFlowRatio);
        }

        private double CalculateMoneyFlow(int index, Func<double, double, bool> addCondition)
        {
            var moneyFlow = 0d;
            for (var i = 0; i < Periods; i++)
            {
                var currentIndex = index - i;
                var currentTypicalPrice = Bars.TypicalPrices[currentIndex];
                var prevTypicalPrice = Bars.TypicalPrices[currentIndex - 1];

                if (addCondition(currentTypicalPrice, prevTypicalPrice))
                    moneyFlow += currentTypicalPrice * Bars.TickVolumes[currentIndex];
            }

            return moneyFlow;
        }
    }
}