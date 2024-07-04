// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class MarketDepthLadder : Indicator
    {
        private string _chartObjectNamesSuffix;

        private Color _buyOrdersColor, _sellOrdersColor;

        private MarketDepth _marketDepth;

        [Parameter("Offset", DefaultValue = 20)]
        public int Offset { get; set; }

        [Parameter("Max Length", DefaultValue = 20, Step = 1, MinValue = 0)]
        public double MaxLength { get; set; }

        [Parameter("Buy Orders Color", DefaultValue = "Lime")]
        public string BuyOrdersColor { get; set; }

        [Parameter("Sell Orders Color", DefaultValue = "Red")]
        public string SellOrdersColor { get; set; }

        [Parameter("Transparency", DefaultValue = 255, MinValue = 0, MaxValue = 255)]
        public int Transparency { get; set; }

        protected override void Initialize()
        {
            _chartObjectNamesSuffix = string.Format("Ladder_{0}", DateTime.Now.Ticks);

            _buyOrdersColor = GetColor(BuyOrdersColor, Transparency);

            _sellOrdersColor = GetColor(SellOrdersColor, Transparency);

            _marketDepth = MarketData.GetMarketDepth(Symbol.Name);
        }

        public override void Calculate(int index)
        {
            if (!IsLastBar)
                return;

            RemoveLadder();

            PlotLadder(index);
        }

        private void PlotLadder(int index)
        {
            int startBarIndex = index + Offset;

            DateTime startTime = index - startBarIndex > 0 ? Bars.OpenTimes[startBarIndex] : GetOpenTime(startBarIndex);

            if (_marketDepth.AskEntries.Count == 0 || _marketDepth.BidEntries.Count == 0)
                return;

            double minBidVolume = _marketDepth.BidEntries.Min(entry => entry.VolumeInUnits);
            double maxBidVolume = _marketDepth.BidEntries.Max(entry => entry.VolumeInUnits);

            double minAskVolume = _marketDepth.AskEntries.Min(entry => entry.VolumeInUnits);
            double maxAskVolume = _marketDepth.AskEntries.Max(entry => entry.VolumeInUnits);

            double min = Math.Min(minBidVolume, minAskVolume);
            double max = Math.Max(maxBidVolume, maxAskVolume);

            double minAllowed = 0;

            foreach (MarketDepthEntry depthEntry in _marketDepth.BidEntries)
            {
                //Print(depthEntry.Price, " | ", depthEntry.VolumeInUnits, " | Bid");

                string objName = string.Format("{0}_Bid_{1}", depthEntry.Price, _chartObjectNamesSuffix);

                double volumeAmount = MinMax(depthEntry.VolumeInUnits, min, max, minAllowed, MaxLength);

                double endIndex = startBarIndex - volumeAmount;

                DateTime endTime = GetOpenTime(endIndex);

                ChartRectangle rectangle = Chart.DrawRectangle(objName, startTime, depthEntry.Price, endTime, depthEntry.Price, _buyOrdersColor, 1, LineStyle.Solid);

                rectangle.IsFilled = true;
            }

            foreach (MarketDepthEntry depthEntry in _marketDepth.AskEntries)
            {
                string objName = string.Format("{0}_Ask_{1}", depthEntry.Price, _chartObjectNamesSuffix);

                double volumeAmount = MinMax(depthEntry.VolumeInUnits, min, max, minAllowed, MaxLength);

                double endIndex = startBarIndex - volumeAmount;

                DateTime endTime = GetOpenTime(endIndex);

                ChartRectangle rectangle = Chart.DrawRectangle(objName, startTime, depthEntry.Price, endTime, depthEntry.Price, _sellOrdersColor, 1, LineStyle.Solid);

                rectangle.IsFilled = true;
            }
        }

        private void RemoveLadder()
        {
            var objectNames = Chart.Objects.Where(chartObject => chartObject.Name.EndsWith(_chartObjectNamesSuffix, StringComparison.InvariantCultureIgnoreCase)).Select(chartObject => chartObject.Name).ToArray();

            foreach (string chartObjectName in objectNames)
            {
                Chart.RemoveObject(chartObjectName);
            }
        }

        private Color GetColor(string colorString, int alpha = 255)
        {
            var color = colorString[0] == '#' ? Color.FromHex(colorString) : Color.FromName(colorString);

            return Color.FromArgb(alpha, color);
        }

        private DateTime GetOpenTime(double barIndex)
        {
            var currentIndex = Bars.Count - 1;

            var timeDiff = GetTimeDiff();

            var indexDiff = barIndex - currentIndex;

            var indexDiffAbs = Math.Abs(indexDiff);

            var result = indexDiff <= 0 ? Bars.OpenTimes[(int)barIndex] : Bars.OpenTimes[currentIndex];

            if (indexDiff > 0)
            {
                for (var i = 1; i <= indexDiffAbs; i++)
                {
                    do
                    {
                        result = result.Add(timeDiff);
                    } while (result.DayOfWeek == DayOfWeek.Saturday || result.DayOfWeek == DayOfWeek.Sunday);
                }
            }

            var barIndexFraction = barIndex % 1;

            var barIndexFractionInMinutes = timeDiff.TotalMinutes * barIndexFraction;

            result = result.AddMinutes(barIndexFractionInMinutes);

            return result;
        }

        private TimeSpan GetTimeDiff()
        {
            var index = Bars.Count - 1;

            if (index < 4)
            {
                throw new InvalidOperationException("Not enough data in market series to calculate the time difference");
            }

            var timeDiffs = new List<TimeSpan>();

            for (var i = index; i >= index - 4; i--)
            {
                timeDiffs.Add(Bars.OpenTimes[i] - Bars.OpenTimes[i - 1]);
            }

            return timeDiffs.GroupBy(diff => diff).OrderBy(diffGroup => diffGroup.Count()).Last().First();
        }

        private double MinMax(double number, double min, double max, double minAllowedNumber, double maxAllowedNumber)
        {
            var b = max - min != 0 ? max - min : 1 / max;
            var uninterpolate = (number - min) / b;
            var result = minAllowedNumber * (1 - uninterpolate) + maxAllowedNumber * uninterpolate;

            return result;
        }
    }
}
