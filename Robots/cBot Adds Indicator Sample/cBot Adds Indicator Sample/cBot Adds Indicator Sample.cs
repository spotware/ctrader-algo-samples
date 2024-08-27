// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This sample cBot adds two moving averages for trading to a chart.  
//
//    For a detailed tutorial on creating this cBot, see this video: https://www.youtube.com/watch?v=DUzdEt30OSE
//
// -------------------------------------------------------------------------------------------------


using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None, AddIndicators = true)]
    public class SampleTrendcBot : Robot
    {
        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }

        [Parameter("MA Type", Group = "Moving Average")]
        public MovingAverageType MAType { get; set; }

        [Parameter("Source", Group = "Moving Average")]
        public DataSeries SourceSeries { get; set; }

        [Parameter("Slow Periods", Group = "Moving Average", DefaultValue = 10)]
        public int SlowPeriods { get; set; }

        [Parameter("Fast Periods", Group = "Moving Average", DefaultValue = 5)]
        public int FastPeriods { get; set; }

        private MovingAverage slowMa;
        private MovingAverage fastMa;
        private const string label = "Sample Trend cBot";   

        ChartIndicator _indicator1;
        ChartIndicator _indicator2;

        protected override void OnStart()
        {
            fastMa = Indicators.MovingAverage(SourceSeries, FastPeriods, MAType);
            slowMa = Indicators.MovingAverage(SourceSeries, SlowPeriods, MAType);

            _indicator1 = Chart.Indicators.Add("Simple Moving Average", SourceSeries, FastPeriods, MAType);
            _indicator2 = Chart.Indicators.Add("Simple Moving Average", SourceSeries, SlowPeriods, MAType);

            _indicator1.Lines[0].Color = Color.Red;
            _indicator1.Lines[0].Thickness = 3;
        }

        protected override void OnBarClosed()
        {
            Chart.Indicators.Remove(_indicator1);
            Chart.Indicators.Remove(_indicator2);
        }

        protected override void OnTick()
        {
            var longPosition = Positions.Find(label, SymbolName, TradeType.Buy);
            var shortPosition = Positions.Find(label, SymbolName, TradeType.Sell);

            var currentSlowMa = slowMa.Result.Last(0);
            var currentFastMa = fastMa.Result.Last(0);
            var previousSlowMa = slowMa.Result.Last(1);
            var previousFastMa = fastMa.Result.Last(1);

            if (previousSlowMa > previousFastMa && currentSlowMa <= currentFastMa && longPosition == null)
            {
                if (shortPosition != null)
                    ClosePosition(shortPosition);

                ExecuteMarketOrder(TradeType.Buy, SymbolName, VolumeInUnits, label);
            }
            else if (previousSlowMa < previousFastMa && currentSlowMa >= currentFastMa && shortPosition == null)
            {
                if (longPosition != null)
                    ClosePosition(longPosition);

                ExecuteMarketOrder(TradeType.Sell, SymbolName, VolumeInUnits, label);
            }
        }

        private double VolumeInUnits
        {
            get { return Symbol.QuantityToVolumeInUnits(Quantity); }
        }
    }
}