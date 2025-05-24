using System;
using System.Collections.Generic;
using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class IntermarketCorrelationIndicator : Indicator
    {
        [Parameter("Correlation Period", DefaultValue = 20)]
        public int CorrelationPeriod { get; set; }
        
        [Parameter("USD Index Symbol", DefaultValue = "USDX")]
        public string UsdIndexSymbol { get; set; }
        
        [Parameter("US 10Y Bond Symbol", DefaultValue = "US10Y")]
        public string BondSymbol { get; set; }
        
        [Output("Gold-USD Correlation", LineColor = "Red")]
        public IndicatorDataSeries GoldUsd { get; set; }
        
        [Output("Gold-Bonds Correlation", LineColor = "Blue")]
        public IndicatorDataSeries GoldBonds { get; set; }
        
        private Bars usdBars;
        private Bars bondBars;
        
        protected override void Initialize()
        {
            try
            {
                // Get bars for related markets
                usdBars = MarketData.GetBars(TimeFrame, UsdIndexSymbol);
                bondBars = MarketData.GetBars(TimeFrame, BondSymbol);
            }
            catch (Exception ex)
            {
                Print($"Error initializing IntermarketCorrelationIndicator: {ex.Message}");
                
                // Use dummy data if symbols not available
                usdBars = Bars;
                bondBars = Bars;
            }
        }
        
        public override void Calculate(int index)
        {
            // Wait for enough bars
            if (index < CorrelationPeriod)
                return;
                
            // Calculate correlation between Gold and USD Index
            GoldUsd[index] = CalculateCorrelation(Bars, usdBars, index, CorrelationPeriod);
            
            // Calculate correlation between Gold and US 10Y Bond
            GoldBonds[index] = CalculateCorrelation(Bars, bondBars, index, CorrelationPeriod);
        }
        
        private double CalculateCorrelation(Bars series1, Bars series2, int currentIndex, int period)
        {
            // Check if we have enough data
            if (currentIndex < period || series2.Count <= currentIndex)
                return 0;
                
            // Extract price data for correlation calculation
            List<double> data1 = new List<double>();
            List<double> data2 = new List<double>();
            
            for (int i = 0; i < period; i++)
            {
                int idx = currentIndex - i;
                if (idx >= 0 && idx < series1.Count && idx < series2.Count)
                {
                    data1.Add(series1.Close[idx]);
                    data2.Add(series2.Close[idx]);
                }
            }
            
            // Calculate Pearson correlation coefficient
            return CalculatePearsonCorrelation(data1, data2);
        }
        
        private double CalculatePearsonCorrelation(List<double> x, List<double> y)
        {
            int n = Math.Min(x.Count, y.Count);
            
            if (n < 2)
                return 0;
                
            // Calculate means
            double meanX = 0;
            double meanY = 0;
            
            for (int i = 0; i < n; i++)
            {
                meanX += x[i];
                meanY += y[i];
            }
            
            meanX /= n;
            meanY /= n;
            
            // Calculate correlation coefficient
            double numerator = 0;
            double denominatorX = 0;
            double denominatorY = 0;
            
            for (int i = 0; i < n; i++)
            {
                double xDiff = x[i] - meanX;
                double yDiff = y[i] - meanY;
                
                numerator += xDiff * yDiff;
                denominatorX += xDiff * xDiff;
                denominatorY += yDiff * yDiff;
            }
            
            if (denominatorX == 0 || denominatorY == 0)
                return 0;
                
            return numerator / (Math.Sqrt(denominatorX) * Math.Sqrt(denominatorY));
        }
    }
}