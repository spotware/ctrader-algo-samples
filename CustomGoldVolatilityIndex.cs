using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CustomGoldVolatilityIndex : Indicator
    {
        #region Parameters

        [Parameter("Historical Volatility Period", DefaultValue = 20)]
        public int HistoricalVolatilityPeriod { get; set; }

        [Parameter("Implied Volatility Weight", DefaultValue = 0.5, MinValue = 0.0, MaxValue = 1.0)]
        public double ImpliedVolatilityWeight { get; set; }

        [Parameter("VIX Symbol", DefaultValue = "VIX")]
        public string VixSymbol { get; set; }

        [Parameter("Gold/USD Correlation Period", DefaultValue = 30)]
        public int GoldUsdCorrelationPeriod { get; set; }

        [Parameter("Volatility Smoothing Period", DefaultValue = 5)]
        public int VolatilitySmoothingPeriod { get; set; }

        #endregion

        #region Outputs

        [Output("Main", LineColor = "Gold", PlotType = PlotType.Line, Thickness = 2)]
        public IndicatorDataSeries Result { get; set; }

        [Output("Historical Volatility", LineColor = "DarkOrange", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries HistoricalVolatility { get; set; }

        [Output("Implied Volatility", LineColor = "Red", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries ImpliedVolatility { get; set; }

        [Output("Volatility Premium", LineColor = "Purple", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries VolatilityPremium { get; set; }

        [Output("Upper Band", LineColor = "DarkRed", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries UpperBand { get; set; }

        [Output("Lower Band", LineColor = "DarkGreen", PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries LowerBand { get; set; }

        #endregion

        #region Private Fields

        private StandardDeviation _stdDev;
        private ExponentialMovingAverage _ema;
        private Bars _vixBars;
        private Bars _usdBars;
        private double _lastVixValue;
        private double _lastUsdValue;
        private List<double> _historicalReturns;
        private List<double> _vixValues;
        private List<double> _usdValues;
        private double _volatilityZScore;
        private double _volatilityPercentile;

        #endregion

        protected override void Initialize()
        {
            // Initialize indicators
            _stdDev = Indicators.StandardDeviation(Bars.ClosePrices, HistoricalVolatilityPeriod, MovingAverageType.Simple);
            _ema = Indicators.ExponentialMovingAverage(Result, VolatilitySmoothingPeriod);
            
            // Initialize collections
            _historicalReturns = new List<double>();
            _vixValues = new List<double>();
            _usdValues = new List<double>();
            
            // Try to get VIX and USD index data
            try
            {
                _vixBars = MarketData.GetBars(TimeFrame, VixSymbol);
                Print("Successfully loaded VIX data");
            }
            catch (Exception ex)
            {
                Print("Error loading VIX data: " + ex.Message);
                _vixBars = null;
            }
            
            try
            {
                _usdBars = MarketData.GetBars(TimeFrame, "USDOLLAR");
                Print("Successfully loaded USD index data");
            }
            catch (Exception ex)
            {
                Print("Error loading USD index data: " + ex.Message);
                _usdBars = null;
            }
        }

        public override void Calculate(int index)
        {
            // Calculate historical volatility (annualized)
            double historicalVol = _stdDev.Result[index] * Math.Sqrt(252) / Bars.ClosePrices[index] * 100;
            HistoricalVolatility[index] = historicalVol;
            
            // Store historical returns for correlation calculation
            if (index > 0)
            {
                double dailyReturn = Math.Log(Bars.ClosePrices[index] / Bars.ClosePrices[index - 1]);
                _historicalReturns.Add(dailyReturn);
                
                // Keep only the required number of values
                if (_historicalReturns.Count > GoldUsdCorrelationPeriod)
                    _historicalReturns.RemoveAt(0);
            }
            
            // Get implied volatility from VIX if available
            double impliedVol = 0;
            if (_vixBars != null && index < _vixBars.ClosePrices.Count)
            {
                impliedVol = _vixBars.ClosePrices[Math.Min(index, _vixBars.ClosePrices.Count - 1)];
                _lastVixValue = impliedVol;
                _vixValues.Add(impliedVol);
                
                // Keep only the required number of values
                if (_vixValues.Count > HistoricalVolatilityPeriod)
                    _vixValues.RemoveAt(0);
            }
            else
            {
                // Use last known value if current is not available
                impliedVol = _lastVixValue;
            }
            
            // Adjust implied volatility based on gold's typical volatility ratio to VIX
            // Gold is typically 1.2-1.5x more volatile than the S&P 500
            impliedVol *= 1.35;
            ImpliedVolatility[index] = impliedVol;
            
            // Get USD index data if available for correlation
            if (_usdBars != null && index < _usdBars.ClosePrices.Count)
            {
                double usdValue = _usdBars.ClosePrices[Math.Min(index, _usdBars.ClosePrices.Count - 1)];
                _lastUsdValue = usdValue;
                _usdValues.Add(usdValue);
                
                // Keep only the required number of values
                if (_usdValues.Count > GoldUsdCorrelationPeriod)
                    _usdValues.RemoveAt(0);
            }
            else
            {
                // Use last known value if current is not available
                _usdValues.Add(_lastUsdValue);
                
                // Keep only the required number of values
                if (_usdValues.Count > GoldUsdCorrelationPeriod)
                    _usdValues.RemoveAt(0);
            }
            
            // Calculate volatility premium (difference between implied and historical)
            double volatilityPremium = impliedVol - historicalVol;
            VolatilityPremium[index] = volatilityPremium;
            
            // Calculate correlation between gold and USD if we have enough data
            double correlation = 0;
            if (_historicalReturns.Count >= GoldUsdCorrelationPeriod && _usdValues.Count >= GoldUsdCorrelationPeriod)
            {
                correlation = CalculatePearsonCorrelation(_historicalReturns, _usdValues);
            }
            
            // Calculate volatility Z-score (how many standard deviations from mean)
            if (_vixValues.Count > 0)
            {
                double mean = _vixValues.Average();
                double stdDev = CalculateStandardDeviation(_vixValues);
                _volatilityZScore = stdDev > 0 ? (impliedVol - mean) / stdDev : 0;
                
                // Calculate volatility percentile
                int count = 0;
                foreach (double val in _vixValues)
                {
                    if (val <= impliedVol)
                        count++;
                }
                _volatilityPercentile = (double)count / _vixValues.Count;
            }
            
            // Combine all factors into the final volatility index
            // 1. Historical volatility
            // 2. Implied volatility (with weight parameter)
            // 3. Volatility premium
            // 4. Gold-USD correlation (negative correlation typically increases gold volatility)
            // 5. Volatility Z-score (extreme values indicate potential volatility spikes)
            
            double finalVolatilityIndex = historicalVol * (1 - ImpliedVolatilityWeight) + 
                                         impliedVol * ImpliedVolatilityWeight + 
                                         volatilityPremium * 0.2 + 
                                         (correlation < 0 ? Math.Abs(correlation) * 5 : 0) + 
                                         Math.Abs(_volatilityZScore) * 2;
            
            // Normalize to a 0-100 scale for easier interpretation
            finalVolatilityIndex = Math.Min(100, Math.Max(0, finalVolatilityIndex));
            
            // Apply smoothing
            Result[index] = finalVolatilityIndex;
            
            // Calculate bands (2 standard deviations from the mean of the volatility index)
            if (index >= HistoricalVolatilityPeriod)
            {
                double sum = 0;
                double sumSquared = 0;
                
                for (int i = index - HistoricalVolatilityPeriod + 1; i <= index; i++)
                {
                    sum += Result[i];
                    sumSquared += Result[i] * Result[i];
                }
                
                double mean = sum / HistoricalVolatilityPeriod;
                double variance = sumSquared / HistoricalVolatilityPeriod - mean * mean;
                double stdDev = Math.Sqrt(variance);
                
                UpperBand[index] = mean + 2 * stdDev;
                LowerBand[index] = Math.Max(0, mean - 2 * stdDev);
            }
            else
            {
                UpperBand[index] = Result[index] * 1.5;
                LowerBand[index] = Result[index] * 0.5;
            }
        }
        
        // Helper method to calculate Pearson correlation coefficient
        private double CalculatePearsonCorrelation(List<double> x, List<double> y)
        {
            if (x.Count != y.Count || x.Count == 0)
                return 0;
                
            int n = x.Count;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0, sumY2 = 0;
            
            for (int i = 0; i < n; i++)
            {
                sumX += x[i];
                sumY += y[i];
                sumXY += x[i] * y[i];
                sumX2 += x[i] * x[i];
                sumY2 += y[i] * y[i];
            }
            
            double denominator = Math.Sqrt((n * sumX2 - sumX * sumX) * (n * sumY2 - sumY * sumY));
            
            if (denominator == 0)
                return 0;
                
            return (n * sumXY - sumX * sumY) / denominator;
        }
        
        // Helper method to calculate standard deviation
        private double CalculateStandardDeviation(List<double> values)
        {
            if (values.Count <= 1)
                return 0;
                
            double avg = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - avg) * (val - avg)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / (values.Count - 1));
        }
        
        // Method to interpret the volatility index value
        public string InterpretVolatility(double volatilityValue)
        {
            if (volatilityValue >= 80)
                return "Extreme Volatility - High risk environment, consider reducing position sizes";
            else if (volatilityValue >= 60)
                return "High Volatility - Increased risk, wider stops recommended";
            else if (volatilityValue >= 40)
                return "Moderate Volatility - Normal trading conditions";
            else if (volatilityValue >= 20)
                return "Low Volatility - Potential for breakout, watch for consolidation patterns";
            else
                return "Very Low Volatility - Consolidation phase, prepare for potential volatility expansion";
        }
        
        // Method to get volatility-based position sizing recommendation
        public double GetVolatilityBasedPositionSizeMultiplier(double volatilityValue, double baseRiskPercentage)
        {
            // Adjust position size inversely to volatility
            // Higher volatility = smaller position size
            double volatilityFactor = 1 - (volatilityValue / 100);
            
            // Ensure we don't reduce position size below 25% of base or increase above 150%
            volatilityFactor = Math.Min(1.5, Math.Max(0.25, volatilityFactor));
            
            return volatilityFactor;
        }
    }
}