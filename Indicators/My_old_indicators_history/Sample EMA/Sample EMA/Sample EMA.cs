// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Automate API example.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AutoRescale = false, AccessRights = AccessRights.None)]
    public class SampleEMA : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Periods", DefaultValue = 14)]
        public int Periods { get; set; }

        [Output("Main", LineColor = "Turquoise")]
        public IndicatorDataSeries Result { get; set; }

        private double exp;

        protected override void Initialize()
        {
            exp = 2.0 / (Periods + 1);
        }

        public override void Calculate(int index)
        {
            var previousValue = Result[index - 1];

            if (double.IsNaN(previousValue))
                Result[index] = Source[index];
            else
                Result[index] = Source[index] * exp + previousValue * (1 - exp);
        }
    }
}
