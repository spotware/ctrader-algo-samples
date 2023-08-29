using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class LevelIndicator : Indicator
    {
        [Parameter(DefaultValue = 10.0)]
        public double Parameter { get; set; }

        [Output("Main", LineColor = "Yellow")]
        public IndicatorDataSeries Result { get; set; }


        protected override void Initialize()
        {
            // Initialize and create nested indicators
        }

        public override void Calculate(int index)
        {
            // Calculate value at specified index
            Result[index] = Bars.ClosePrices[index];
        }
    }
}