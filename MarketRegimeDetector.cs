using System;
using System.Collections.Generic;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo.Indicators
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class MarketRegimeDetector : Indicator
    {   
        [Parameter("Volatility Period", DefaultValue = 20)]
        public int VolatilityPeriod { get; set; }
        
        [Parameter("Trend Period", DefaultValue = 50)]
        public int TrendPeriod { get; set; }
        
        [Parameter("Inflation Symbol", DefaultValue = "US10Y")]
        public string InflationSymbol { get; set; }
        
        [Parameter("Risk Sentiment Symbol", DefaultValue = "SPX500")]
        public string RiskSentimentSymbol { get; set; }
        
        [Output("Regime", LineColor = "Gold")]
        public IndicatorDataSeries RegimeValue { get; set; }
        
        [Output("Volatility Regime", LineColor = "Red")]
        public IndicatorDataSeries VolatilityRegime { get; set; }
        
        [Output("Trend Regime", LineColor = "Blue")]
        public IndicatorDataSeries TrendRegime { get; set; }
        
        [Output("Inflation Regime", LineColor = "Green")]
        public IndicatorDataSeries InflationRegime { get; set; }
        
        [Output("Risk Regime", LineColor = "Purple")]
        public IndicatorDataSeries RiskRegime { get; set; }
        
        private MovingAverage longMA;
        private MovingAverage shortMA;
        private AverageTrueRange atr;
        private Bars inflationBars;
        private Bars riskBars;
        
        // Regime constants
        public const double REGIME_BULLISH_TRENDING = 1.0;
        public const double REGIME_BEARISH_TRENDING = -1.0;
        public const double REGIME_HIGH_VOLATILITY = 0.5;
        public const double REGIME_LOW_VOLATILITY = -0.5;
        public const double REGIME_INFLATION_RISING = 0.75;
        public const double REGIME_INFLATION_FALLING = -0.75;
        public const double REGIME_RISK_ON = 0.25;
        public const double REGIME_RISK_OFF = -0.25;
        
        protected override void Initialize()
        {   
            // Initialize indicators
            longMA = Indicators.MovingAverage(Bars.Close, TrendPeriod, MovingAverageType.Exponential);
            shortMA = Indicators.MovingAverage(Bars.Close, TrendPeriod / 5, MovingAverageType.Exponential);
            atr = Indicators.AverageTrueRange(VolatilityPeriod);
            
            try
            {
                // Get bars for related markets
                inflationBars = MarketData.GetBars(TimeFrame, InflationSymbol);
                riskBars = MarketData.GetBars(TimeFrame, RiskSentimentSymbol);
            }
            catch (Exception ex)
            {
                Print($"Error initializing MarketRegimeDetector: {ex.Message}");
                
                // Use dummy data if symbols not available
                inflationBars = Bars;
                riskBars = Bars;
            }
        }
        
        public override void Calculate(int index)
        {   
            // Wait for enough bars
            if (index < Math.Max(TrendPeriod, VolatilityPeriod))
                return;
            
            // Detect trend regime
            double trendRegime = DetectTrendRegime(index);
            TrendRegime[index] = trendRegime;
            
            // Detect volatility regime
            double volatilityRegime = DetectVolatilityRegime(index);
            VolatilityRegime[index] = volatilityRegime;
            
            // Detect inflation regime
            double inflationRegime = DetectInflationRegime(index);
            InflationRegime[index] = inflationRegime;
            
            // Detect risk sentiment regime
            double riskRegime = DetectRiskRegime(index);
            RiskRegime[index] = riskRegime;
            
            // Combine regimes into a single value
            // This is a simplified approach - in a real system, you might use machine learning
            // to determine the optimal weights for each regime component
            RegimeValue[index] = (trendRegime * 0.4) + (volatilityRegime * 0.3) + 
                                (inflationRegime * 0.2) + (riskRegime * 0.1);
        }
        
        private double DetectTrendRegime(int index)
        {   
            // Simple trend detection using MA crossover
            if (shortMA.Result[index] > longMA.Result[index] && 
                shortMA.Result[index-1] <= longMA.Result[index-1])
            {
                return REGIME_BULLISH_TRENDING;
            }
            else if (shortMA.Result[index] < longMA.Result[index] && 
                     shortMA.Result[index-1] >= longMA.Result[index-1])
            {
                return REGIME_BEARISH_TRENDING;
            }
            
            // Continue existing trend
            if (shortMA.Result[index] > longMA.Result[index])
            {
                return REGIME_BULLISH_TRENDING;
            }
            else
            {
                return REGIME_BEARISH_TRENDING;
            }
        }
        
        private double DetectVolatilityRegime(int index)
        {   
            // Calculate historical volatility percentile
            double currentATR = atr.Result[index];
            double historicalMeanATR = 0;
            double historicalStdATR = 0;
            
            // Calculate mean and standard deviation of ATR
            List<double> atrValues = new List<double>();
            for (int i = 0; i < 100; i++)
            {   
                int idx = index - i;
                if (idx >= 0)
                {
                    atrValues.Add(atr.Result[idx]);
                    historicalMeanATR += atr.Result[idx];
                }
            }
            
            int count = atrValues.Count;
            historicalMeanATR /= count;
            
            foreach (double val in atrValues)
            {
                historicalStdATR += Math.Pow(val - historicalMeanATR, 2);
            }
            
            historicalStdATR = Math.Sqrt(historicalStdATR / count);
            
            // Calculate z-score
            double zScore = (currentATR - historicalMeanATR) / historicalStdATR;
            
            // Determine volatility regime
            if (zScore > 1.0)
            {
                return REGIME_HIGH_VOLATILITY;
            }
            else
            {
                return REGIME_LOW_VOLATILITY;
            }
        }
        
        private double DetectInflationRegime(int index)
        {   
            // Check if we have enough data
            if (index < 20 || inflationBars.Count <= index)
                return 0;
            
            // Use bond yields as a proxy for inflation expectations
            // Rising yields often indicate rising inflation expectations
            double currentYield = inflationBars.Close[index];
            double previousYield = inflationBars.Close[index - 20];
            
            if (currentYield > previousYield * 1.05) // 5% increase in yields
            {
                return REGIME_INFLATION_RISING;
            }
            else if (currentYield < previousYield * 0.95) // 5% decrease in yields
            {
                return REGIME_INFLATION_FALLING;
            }
            
            // Continue existing regime
            if (currentYield > previousYield)
            {
                return REGIME_INFLATION_RISING;
            }
            else
            {
                return REGIME_INFLATION_FALLING;
            }
        }
        
        private double DetectRiskRegime(int index)
        {   
            // Check if we have enough data
            if (index < 20 || riskBars.Count <= index)
                return 0;
            
            // Use equity index as a proxy for risk sentiment
            double currentLevel = riskBars.Close[index];
            double previousLevel = riskBars.Close[index - 20];
            
            if (currentLevel > previousLevel * 1.03) // 3% increase in equities
            {
                return REGIME_RISK_ON;
            }
            else if (currentLevel < previousLevel * 0.97) // 3% decrease in equities
            {
                return REGIME_RISK_OFF;
            }
            
            // Continue existing regime
            if (currentLevel > previousLevel)
            {
                return REGIME_RISK_ON;
            }
            else
            {
                return REGIME_RISK_OFF;
            }
        }
        
        // Method to get the current regime as a string (for display purposes)
        public string GetRegimeDescription(int index)
        {   
            double regimeValue = RegimeValue[index];
            
            if (regimeValue > 0.5)
                return "Bullish Gold Environment";
            else if (regimeValue > 0.2)
                return "Moderately Bullish Gold";
            else if (regimeValue > -0.2)
                return "Neutral Gold Environment";
            else if (regimeValue > -0.5)
                return "Moderately Bearish Gold";
            else
                return "Bearish Gold Environment";
        }
    }
}