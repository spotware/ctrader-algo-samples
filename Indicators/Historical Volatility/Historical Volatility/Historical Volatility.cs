using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(ScalePrecision = 3, AccessRights = AccessRights.None)]
    public class HistoricalVolatility : Indicator
    {
        private IndicatorDataSeries _logarithms;
        private StandardDeviation _standardDeviation;

        [Parameter]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 20, MinValue = 1)]
        public int Periods { get; set; }

        [Parameter("Bar History", DefaultValue = 252)]
        public int BarHistory { get; set; }

        [Output("Main", LineColor = "Orange")]
        public IndicatorDataSeries Result { get; set; }

        protected override void Initialize()
        {
            _logarithms = CreateDataSeries();
            _standardDeviation = Indicators.StandardDeviation(_logarithms, Periods, MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            _logarithms[index] = Math.Log10(Source[index] / Source[index - 1]);
            Result[index] = _standardDeviation.Result[index] * Math.Sqrt(BarHistory);
        }
    }
}