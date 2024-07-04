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
using System;

namespace cAlgo.API
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None), Cloud("Top", "Bottom", Opacity = 0.2, FirstColor = "Blue", SecondColor = "Green")]
    public class BollingerBandsMTFCloudSample : Indicator
    {
        private BollingerBands _bollingerBands;

        private Bars _baseBars;

        [Parameter("Base TimeFrame", DefaultValue = "Daily")]
        public TimeFrame BaseTimeFrame { get; set; }

        [Parameter("Source", DefaultValue = DataSeriesType.Close)]
        public DataSeriesType DataSeriesType { get; set; }

        [Parameter("Periods", DefaultValue = 14, MinValue = 0)]
        public int Periods { get; set; }

        [Parameter("Standard Deviation", DefaultValue = 2, MinValue = 0)]
        public double StandardDeviation { get; set; }

        [Parameter("MA Type", DefaultValue = MovingAverageType.Simple)]
        public MovingAverageType MaType { get; set; }

        [Output("Main", LineColor = "Yellow", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Main { get; set; }

        [Output("Top", LineColor = "Red", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Top { get; set; }

        [Output("Bottom", LineColor = "Red", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Bottom { get; set; }

        protected override void Initialize()
        {
            _baseBars = MarketData.GetBars(BaseTimeFrame);

            var baseSeries = GetBaseSeries();

            _bollingerBands = Indicators.BollingerBands(baseSeries, Periods, StandardDeviation, MaType);
        }

        public override void Calculate(int index)
        {
            var baseIndex = _baseBars.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

            Main[index] = _bollingerBands.Main[baseIndex];
            Top[index] = _bollingerBands.Top[baseIndex];
            Bottom[index] = _bollingerBands.Bottom[baseIndex];
        }

        private DataSeries GetBaseSeries()
        {
            switch (DataSeriesType)
            {
                case DataSeriesType.Open:
                    return _baseBars.OpenPrices;

                case DataSeriesType.High:
                    return _baseBars.HighPrices;

                case DataSeriesType.Low:
                    return _baseBars.LowPrices;

                case DataSeriesType.Close:
                    return _baseBars.ClosePrices;

                default:

                    throw new ArgumentOutOfRangeException("DataSeriesType");
            }
        }
    }

    public enum DataSeriesType
    {
        Open,
        High,
        Low,
        Close
    }
}