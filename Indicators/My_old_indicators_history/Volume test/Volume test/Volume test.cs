// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Automate API example.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Indicator(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleBullsPower : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter(DefaultValue = 13, MinValue = 2)]
        public int Periods { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MAType { get; set; }

        [Output("Result1", LineColor = "Red", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Result1 { get; set; }
        
        [Output("Result2", LineColor = "Green", PlotType = PlotType.Histogram)]
        public IndicatorDataSeries Result2 { get; set; }
       
        private MovingAverage movingAverage;
        private TickVolume volumeData;

        protected override void Initialize()
        {
            movingAverage = Indicators.MovingAverage(Source, Periods, MAType);
            volumeData = Indicators.TickVolume();

        }

        public override void Calculate(int index)
        {
            //if (index % 2 == 0)
            if (Bars.ClosePrices[index] < Bars.OpenPrices[index])
            {
                //Result1[index] = Bars.HighPrices[index] - movingAverage.Result[index];
                Result1[index] = volumeData.Result[index];
            }
            else
            {
                //Result2[index] = Bars.HighPrices[index] - movingAverage.Result[index];
                Result2[index] = volumeData.Result[index];
            }
            
        }
    }
}
