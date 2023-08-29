using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class MultiSessionOpen : Indicator
    {
        [Parameter("London Session", DefaultValue = true)]
        public bool london { get; set; }
        [Parameter("NY Session", DefaultValue = true)]
        public bool NY { get; set; }
        [Parameter("Sydney Session", DefaultValue = true)]
        public bool sydney { get; set; }
        [Parameter("Tokyo Session", DefaultValue = true)]
        public bool tokyo { get; set; }

        [Parameter("Show Open Prices", DefaultValue = true)]
        public bool openPrices { get; set; }

        [Parameter("Show Info", DefaultValue = true)]
        public bool info { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }

        private MarketSeries openSeries;


        protected override void Initialize()
        {
            openSeries = MarketData.GetSeries(TimeFrame.Hour);
        }

        public override void Calculate(int index)
        {
            //London Session
            string today = MarketSeries.OpenTime[index].Day.ToString() + "/" + MarketSeries.OpenTime[index].Month.ToString() + "/" + MarketSeries.OpenTime[index].Year.ToString() + " ";
            if (london)
            {
                DateTime londonOpen, londonClose;
                DateTime.TryParse(today + "07:00:00", out londonOpen);
                DateTime.TryParse(today + "15:00:00", out londonClose);
                double londonMax = 0, londonMin = double.PositiveInfinity;
                londonOpen = londonOpen.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                londonClose = londonClose.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                for (int i = MarketSeries.OpenTime.GetIndexByTime(londonOpen); i < MarketSeries.OpenTime.GetIndexByTime(londonClose); i++)
                {
                    londonMax = Math.Max(londonMax, MarketSeries.High[i]);
                    londonMin = Math.Min(londonMin, MarketSeries.Low[i]);
                }
                double londonPrice = Math.Round((MarketSeries.Close[index] - openSeries.Open[MarketSeries.OpenTime.GetIndexByTime(londonOpen)]) / Symbol.PipSize, 2);
                Chart.DrawRectangle("london session " + londonOpen, londonOpen, londonMax, londonClose, londonMin, Color.FromArgb(50, 0, 50, 255)).IsFilled = true;
                if (openPrices)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= londonOpen)
                            Chart.DrawTrendLine("_open london" + londonOpen, londonOpen, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(londonOpen)], londonClose, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(londonOpen)], Color.Blue);
                if (info)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= londonOpen)
                            Chart.DrawStaticText("london info", "Pips From London Open: " + londonPrice, VerticalAlignment.Top, HorizontalAlignment.Right, Color.RoyalBlue);
                        else
                            Chart.DrawStaticText("london info", "Pips From London Open: Waiting for Open", VerticalAlignment.Top, HorizontalAlignment.Right, Color.RoyalBlue);

            }

            //NY Session
            if (NY)
            {
                DateTime nyOpen, nyClose;
                DateTime.TryParse(today + "12:00:00", out nyOpen);
                DateTime.TryParse(today + "20:00:00", out nyClose);
                double nyMax = 0, nyMin = double.PositiveInfinity;
                nyOpen = nyOpen.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                nyClose = nyClose.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                for (int i = MarketSeries.OpenTime.GetIndexByTime(nyOpen); i < MarketSeries.OpenTime.GetIndexByTime(nyClose); i++)
                {
                    nyMax = Math.Max(nyMax, MarketSeries.High[i]);
                    nyMin = Math.Min(nyMin, MarketSeries.Low[i]);
                }
                double nyPrice = Math.Round((MarketSeries.Close[index] - openSeries.Open[MarketSeries.OpenTime.GetIndexByTime(nyOpen)]) / Symbol.PipSize, 2);
                Chart.DrawRectangle("ny session " + nyOpen, nyOpen, nyMax, nyClose, nyMin, Color.FromArgb(50, 255, 50, 0)).IsFilled = true;
                if (openPrices)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= nyOpen)
                            Chart.DrawTrendLine("_open ny" + nyOpen, nyOpen, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(nyOpen)], nyClose, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(nyOpen)], Color.Red);
                if (info)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= nyOpen)
                            Chart.DrawStaticText("ny info", "\nPips From NY Open: " + nyPrice, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
                        else
                            Chart.DrawStaticText("ny info", "\nPips From NY Open: Waiting for Open", VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);


            }

            //Sydney Session
            string yesterday = MarketSeries.OpenTime[index].AddDays(-1).Day.ToString() + "/" + MarketSeries.OpenTime[index].AddDays(-1).Month.ToString() + "/" + MarketSeries.OpenTime[index].AddDays(-1).Year.ToString() + " ";
            string tommorow = MarketSeries.OpenTime[index].AddDays(1).Day.ToString() + "/" + MarketSeries.OpenTime[index].AddDays(1).Month.ToString() + "/" + MarketSeries.OpenTime[index].AddDays(1).Year.ToString() + " ";
            if (sydney)
            {
                DateTime sydneyOpen, sydneyClose;
                DateTime.TryParse(MarketSeries.OpenTime[index].Hour <= 6 ? yesterday + "22:00:00" : today + "22:00:00", out sydneyOpen);
                DateTime.TryParse(MarketSeries.OpenTime[index].Hour <= 6 ? today + "6:00:00" : tommorow + "6:00:00", out sydneyClose);
                double sydneyMax = 0, sydneyMin = double.PositiveInfinity;
                sydneyOpen = sydneyOpen.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                sydneyClose = sydneyClose.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                for (int i = MarketSeries.OpenTime.GetIndexByTime(sydneyOpen); i < MarketSeries.OpenTime.GetIndexByTime(sydneyClose); i++)
                {
                    sydneyMax = Math.Max(sydneyMax, MarketSeries.High[i]);
                    sydneyMin = Math.Min(sydneyMin, MarketSeries.Low[i]);
                }
                double sydneyPrice = Math.Round((MarketSeries.Close[index] - openSeries.Open[MarketSeries.OpenTime.GetIndexByTime(sydneyOpen)]) / Symbol.PipSize, 2);
                Chart.DrawRectangle("sydney session " + sydneyOpen, sydneyOpen, sydneyMax, sydneyClose, sydneyMin, Color.FromArgb(50, 50, 255, 0)).IsFilled = true;
                if (openPrices)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= sydneyOpen)
                            Chart.DrawTrendLine("_open sydney" + sydneyOpen, sydneyOpen, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(sydneyOpen)], sydneyClose, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(sydneyOpen)], Color.LawnGreen);
                if (info)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= sydneyOpen)
                            Chart.DrawStaticText("sydney info", "\n\nPips From Sydney Open: " + sydneyPrice, VerticalAlignment.Top, HorizontalAlignment.Right, Color.LawnGreen);

            }

            //Tokyo Session
            if (tokyo)
            {
                DateTime tokyoOpen, tokyoClose;
                DateTime.TryParse(MarketSeries.OpenTime[index].Hour <= 7 ? yesterday + "23:00:00" : today + "23:00:00", out tokyoOpen);
                DateTime.TryParse(MarketSeries.OpenTime[index].Hour <= 7 ? today + "7:00:00" : tommorow + "7:00:00", out tokyoClose);
                double tokyoMax = 0, tokyoMin = double.PositiveInfinity;
                tokyoOpen = tokyoOpen.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                tokyoClose = tokyoClose.AddHours(MarketSeries.OpenTime[index].IsDaylightSavingTime() ? 1 : 0);
                for (int i = MarketSeries.OpenTime.GetIndexByTime(tokyoOpen); i < MarketSeries.OpenTime.GetIndexByTime(tokyoClose); i++)
                {
                    tokyoMax = Math.Max(tokyoMax, MarketSeries.High[i]);
                    tokyoMin = Math.Min(tokyoMin, MarketSeries.Low[i]);
                }
                double tokyoPrice = Math.Round((MarketSeries.Close[index] - openSeries.Open[MarketSeries.OpenTime.GetIndexByTime(tokyoOpen)]) / Symbol.PipSize, 2);
                Chart.DrawRectangle("tokyo session " + tokyoOpen, tokyoOpen, tokyoMax, tokyoClose, tokyoMin, Color.FromArgb(50, 255, 255, 50)).IsFilled = true;
                if (openPrices)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= tokyoOpen)
                            Chart.DrawTrendLine("_open tokyo" + tokyoOpen, tokyoOpen, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(tokyoOpen)], tokyoClose, MarketSeries.Open[MarketSeries.OpenTime.GetIndexByTime(tokyoOpen)], Color.Yellow);
                if (info)
                    if (TimeFrame <= TimeFrame.Hour)
                        if (MarketSeries.OpenTime[index] >= tokyoOpen)
                            Chart.DrawStaticText("tokyo info", "\n\n\nPips From Tokyo Open: " + tokyoPrice, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Yellow);

            }

        }
    }
}
