using System;
using System.Collections.Generic;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo.Robots
{
    public class GoldFeatureEngineering
    {   
        private readonly Bars _goldBars;
        private readonly Symbol _goldSymbol;
        private readonly MarketData _marketData;
        private readonly TimeFrame _timeFrame;
        
        // Related markets
        private Bars _silverBars;
        private Bars _usdIndexBars;
        private Bars _bondBars;
        private Bars _spxBars;
        private Bars _vixBars;
        
        // Indicators
        private MovingAverage _goldEma20;
        private MovingAverage _goldEma50;
        private MovingAverage _goldEma200;
        private RelativeStrengthIndex _goldRsi;
        private BollingerBands _goldBB;
        private MacdCrossOver _goldMacd;
        private AverageTrueRange _goldAtr;
        private MarketRegimeDetector _regimeDetector;
        
        // Constants for symbols
        private const string SILVER_SYMBOL = "XAGUSD";
        private const string USD_INDEX_SYMBOL = "USDX";
        private const string BOND_SYMBOL = "US10Y";
        private const string SPX_SYMBOL = "SPX500";
        private const string VIX_SYMBOL = "VIX";
        
        // Constructor
        public GoldFeatureEngineering(Bars goldBars, Symbol goldSymbol, MarketData marketData, TimeFrame timeFrame, Indicators indicators)
        {   
            _goldBars = goldBars;
            _goldSymbol = goldSymbol;
            _marketData = marketData;
            _timeFrame = timeFrame;
            
            // Initialize indicators
            _goldEma20 = indicators.MovingAverage(goldBars.Close, 20, MovingAverageType.Exponential);
            _goldEma50 = indicators.MovingAverage(goldBars.Close, 50, MovingAverageType.Exponential);
            _goldEma200 = indicators.MovingAverage(goldBars.Close, 200, MovingAverageType.Exponential);
            _goldRsi = indicators.RelativeStrengthIndex(goldBars.Close, 14);
            _goldBB = indicators.BollingerBands(goldBars.Close, 20, 2);
            _goldMacd = indicators.MacdCrossOver(goldBars.Close, 12, 26, 9);
            _goldAtr = indicators.AverageTrueRange(14);
            _regimeDetector = indicators.GetIndicator<MarketRegimeDetector>();
            
            // Initialize related market data
            try
            {   
                _silverBars = marketData.GetBars(timeFrame, SILVER_SYMBOL);
                _usdIndexBars = marketData.GetBars(timeFrame, USD_INDEX_SYMBOL);
                _bondBars = marketData.GetBars(timeFrame, BOND_SYMBOL);
                _spxBars = marketData.GetBars(timeFrame, SPX_SYMBOL);
                _vixBars = marketData.GetBars(timeFrame, VIX_SYMBOL);
            }
            catch (Exception ex)
            {   
                System.Diagnostics.Debug.WriteLine($"Error initializing market data: {ex.Message}");
                // Use gold bars as fallback for any missing data
                if (_silverBars == null) _silverBars = goldBars;
                if (_usdIndexBars == null) _usdIndexBars = goldBars;
                if (_bondBars == null) _bondBars = goldBars;
                if (_spxBars == null) _spxBars = goldBars;
                if (_vixBars == null) _vixBars = goldBars;
            }
        }
        
        // Extract all features for ML model
        public Dictionary<string, double> ExtractFeatures(int index)
        {   
            var features = new Dictionary<string, double>();
            
            // Basic price features
            AddPriceFeatures(features, index);
            
            // Technical indicator features
            AddIndicatorFeatures(features, index);
            
            // Intermarket features
            AddIntermarketFeatures(features, index);
            
            // Market regime features
            AddRegimeFeatures(features, index);
            
            // Seasonal features
            AddSeasonalFeatures(features, index);
            
            // Volatility features
            AddVolatilityFeatures(features, index);
            
            return features;
        }
        
        // Convert features dictionary to array for ML model
        public double[] GetFeaturesArray(Dictionary<string, double> features)
        {   
            // Define the order of features for the ML model
            string[] featureOrder = new string[]
            {
                // Price features
                "close", "open", "high", "low", "volume",
                "close_change_pct", "high_low_range", "close_to_high", "close_to_low",
                
                // Technical indicators
                "ema20", "ema50", "ema200", 
                "ema20_dist", "ema50_dist", "ema200_dist",
                "rsi", "bb_width", "bb_position", 
                "macd", "macd_signal", "macd_hist",
                "atr", "atr_pct",
                
                // Intermarket
                "gold_silver_ratio", "gold_silver_ratio_change",
                "gold_usd_correlation", "gold_bonds_correlation",
                "real_interest_rate", "real_rate_change",
                "risk_sentiment", "vix_level",
                
                // Regime
                "trend_regime", "volatility_regime", "inflation_regime", "risk_regime",
                
                // Seasonal
                "month", "day_of_week", "hour_of_day", "is_fomc_week",
                
                // Volatility
                "historical_vol", "implied_vol", "vol_premium"
            };
            
            // Create array with same order as expected by the model
            var result = new double[featureOrder.Length];
            for (int i = 0; i < featureOrder.Length; i++)
            {   
                if (features.ContainsKey(featureOrder[i]))
                {
                    result[i] = features[featureOrder[i]];
                }
                else
                {   
                    // Use 0 as default for missing features
                    result[i] = 0;
                }
            }
            
            return result;
        }
        
        private void AddPriceFeatures(Dictionary<string, double> features, int index)
        {   
            // Basic price data
            features["close"] = _goldBars.Close[index];
            features["open"] = _goldBars.Open[index];
            features["high"] = _goldBars.High[index];
            features["low"] = _goldBars.Low[index];
            features["volume"] = _goldBars.TickVolume[index];
            
            // Derived price features
            if (index > 0)
            {
                features["close_change_pct"] = (_goldBars.Close[index] / _goldBars.Close[index - 1] - 1) * 100;
            }
            else
            {
                features["close_change_pct"] = 0;
            }
            
            features["high_low_range"] = (_goldBars.High[index] - _goldBars.Low[index]) / _goldBars.Close[index] * 100;
            features["close_to_high"] = (_goldBars.High[index] - _goldBars.Close[index]) / (_goldBars.High[index] - _goldBars.Low[index]);
            features["close_to_low"] = (_goldBars.Close[index] - _goldBars.Low[index]) / (_goldBars.High[index] - _goldBars.Low[index]);
        }
        
        private void AddIndicatorFeatures(Dictionary<string, double> features, int index)
        {   
            // Moving averages
            features["ema20"] = _goldEma20.Result[index];
            features["ema50"] = _goldEma50.Result[index];
            features["ema200"] = _goldEma200.Result[index];
            
            // Distance from moving averages (normalized)
            features["ema20_dist"] = (_goldBars.Close[index] / _goldEma20.Result[index] - 1) * 100;
            features["ema50_dist"] = (_goldBars.Close[index] / _goldEma50.Result[index] - 1) * 100;
            features["ema200_dist"] = (_goldBars.Close[index] / _goldEma200.Result[index] - 1) * 100;
            
            // RSI
            features["rsi"] = _goldRsi.Result[index];
            
            // Bollinger Bands
            features["bb_width"] = (_goldBB.Upper[index] - _goldBB.Lower[index]) / _goldBB.Main[index] * 100;
            features["bb_position"] = (_goldBars.Close[index] - _goldBB.Lower[index]) / (_goldBB.Upper[index] - _goldBB.Lower[index]);
            
            // MACD
            features["macd"] = _goldMacd.MACD[index];
            features["macd_signal"] = _goldMacd.Signal[index];
            features["macd_hist"] = _goldMacd.Histogram[index];
            
            // ATR
            features["atr"] = _goldAtr.Result[index];
            features["atr_pct"] = _goldAtr.Result[index] / _goldBars.Close[index] * 100;
        }
        
        private void AddIntermarketFeatures(Dictionary<string, double> features, int index)
        {   
            // Gold/Silver ratio (key for precious metals analysis)
            if (_silverBars.Count > index && _silverBars.Close[index] > 0)
            {
                features["gold_silver_ratio"] = _goldBars.Close[index] / _silverBars.Close[index];
                
                if (index > 0 && _silverBars.Count > index - 1 && _silverBars.Close[index - 1] > 0)
                {
                    double prevRatio = _goldBars.Close[index - 1] / _silverBars.Close[index - 1];
                    features["gold_silver_ratio_change"] = (features["gold_silver_ratio"] / prevRatio - 1) * 100;
                }
                else
                {
                    features["gold_silver_ratio_change"] = 0;
                }
            }
            else
            {
                features["gold_silver_ratio"] = 0;
                features["gold_silver_ratio_change"] = 0;
            }
            
            // USD correlation
            if (_usdIndexBars.Count > index)
            {
                features["gold_usd_correlation"] = CalculateCorrelation(_goldBars, _usdIndexBars, index, 20);
            }
            else
            {
                features["gold_usd_correlation"] = 0;
            }
            
            // Bond correlation
            if (_bondBars.Count > index)
            {
                features["gold_bonds_correlation"] = CalculateCorrelation(_goldBars, _bondBars, index, 20);
                
                // Real interest rate approximation (bond yield - inflation)
                // Note: In a real system, you would use actual inflation data
                // This is a simplified proxy using bond yields and gold price changes
                features["real_interest_rate"] = _bondBars.Close[index] - (index > 20 ? 
                    (_goldBars.Close[index] / _goldBars.Close[index - 20] - 1) * 100 : 0);
                
                if (index > 0 && _bondBars.Count > index - 1)
                {
                    double prevRealRate = _bondBars.Close[index - 1] - (index > 21 ? 
                        (_goldBars.Close[index - 1] / _goldBars.Close[index - 21] - 1) * 100 : 0);
                    features["real_rate_change"] = features["real_interest_rate"] - prevRealRate;
                }
                else
                {
                    features["real_rate_change"] = 0;
                }
            }
            else
            {
                features["gold_bonds_correlation"] = 0;
                features["real_interest_rate"] = 0;
                features["real_rate_change"] = 0;
            }
            
            // Risk sentiment (SPX)
            if (_spxBars.Count > index)
            {
                features["risk_sentiment"] = index > 0 ? 
                    (_spxBars.Close[index] / _spxBars.Close[index - 1] - 1) * 100 : 0;
            }
            else
            {
                features["risk_sentiment"] = 0;
            }
            
            // VIX (volatility index)
            if (_vixBars.Count > index)
            {
                features["vix_level"] = _vixBars.Close[index];
            }
            else
            {
                features["vix_level"] = 0;
            }
        }
        
        private void AddRegimeFeatures(Dictionary<string, double> features, int index)
        {   
            // Market regime features
            features["trend_regime"] = _regimeDetector.TrendRegime[index];
            features["volatility_regime"] = _regimeDetector.VolatilityRegime[index];
            features["inflation_regime"] = _regimeDetector.InflationRegime[index];
            features["risk_regime"] = _regimeDetector.RiskRegime[index];
        }
        
        private void AddSeasonalFeatures(Dictionary<string, double> features, int index)
        {   
            // Time-based features
            DateTime barTime = _goldBars.OpenTime[index];
            
            features["month"] = barTime.Month;
            features["day_of_week"] = (int)barTime.DayOfWeek;
            features["hour_of_day"] = barTime.Hour;
            
            // FOMC meeting week feature (simplified - in a real system you would use an economic calendar)
            // This is a placeholder - you would need to implement actual FOMC calendar logic
            features["is_fomc_week"] = IsFomcMeetingWeek(barTime) ? 1 : 0;
        }
        
        private void AddVolatilityFeatures(Dictionary<string, double> features, int index)
        {   
            // Historical volatility (using ATR as proxy)
            features["historical_vol"] = features["atr_pct"];
            
            // Implied volatility (using VIX as proxy for market implied vol)
            // In a real system, you would use gold options implied volatility
            features["implied_vol"] = features["vix_level"];
            
            // Volatility risk premium (difference between implied and realized vol)
            features["vol_premium"] = features["implied_vol"] - features["historical_vol"];
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
        
        // Placeholder for FOMC meeting detection
        // In a real system, you would implement a proper economic calendar
        private bool IsFomcMeetingWeek(DateTime date)
        {   
            // This is a simplified placeholder
            // FOMC typically meets 8 times per year, roughly every 6 weeks
            // You would replace this with actual FOMC calendar data
            int month = date.Month;
            int dayOfMonth = date.Day;
            
            // Rough approximation of FOMC meeting weeks
            return (month == 1 && dayOfMonth >= 25 && dayOfMonth <= 31) || // Late January
                   (month == 3 && dayOfMonth >= 15 && dayOfMonth <= 21) || // Mid March
                   (month == 5 && dayOfMonth >= 1 && dayOfMonth <= 7) ||   // Early May
                   (month == 6 && dayOfMonth >= 12 && dayOfMonth <= 18) || // Mid June
                   (month == 7 && dayOfMonth >= 25 && dayOfMonth <= 31) || // Late July
                   (month == 9 && dayOfMonth >= 15 && dayOfMonth <= 21) || // Mid September
                   (month == 11 && dayOfMonth >= 1 && dayOfMonth <= 7) ||  // Early November
                   (month == 12 && dayOfMonth >= 12 && dayOfMonth <= 18);  // Mid December
        }
    }
}