using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class GoldTradingStrategy : Robot
    {
        // Parameters
        [Parameter("RVI Period", DefaultValue = 10)]
        public int RviPeriod { get; set; }
        
        [Parameter("Ichimoku Tenkan", DefaultValue = 9)]
        public int TenkanPeriod { get; set; }
        
        [Parameter("Ichimoku Kijun", DefaultValue = 26)]
        public int KijunPeriod { get; set; }
        
        [Parameter("Ichimoku Senkou Span B", DefaultValue = 52)]
        public int SenkouSpanBPeriod { get; set; }
        
        [Parameter("VWAP Period", DefaultValue = 14)]
        public int VwapPeriod { get; set; }
        
        [Parameter("Gold Volatility Period", DefaultValue = 14)]
        public int GoldVolatilityPeriod { get; set; }
        
        [Parameter("Gold Volatility Threshold", DefaultValue = 0.5, MinValue = 0.1, MaxValue = 2.0)]
        public double GoldVolatilityThreshold { get; set; }
        
        [Parameter("Risk Percentage", DefaultValue = 1.0, MinValue = 0.1, MaxValue = 5.0)]
        public double RiskPercentage { get; set; }
        
        [Parameter("Stop Loss Pips", DefaultValue = 50, MinValue = 10, MaxValue = 200)]
        public int StopLossPips { get; set; }
        
        [Parameter("Take Profit Pips", DefaultValue = 100, MinValue = 20, MaxValue = 400)]
        public int TakeProfitPips { get; set; }
        
        [Parameter("Use Trailing Stop", DefaultValue = true)]
        public bool UseTrailingStop { get; set; }
        
        [Parameter("Trailing Stop Distance", DefaultValue = 30, MinValue = 10, MaxValue = 100)]
        public int TrailingStopDistance { get; set; }
        
        [Parameter("ML Model Path", DefaultValue = "C:\\Models\\gold_model.pkl")]
        public string ModelPath { get; set; }
        
        [Parameter("Multi-Timeframe Confirmation", DefaultValue = true)]
        public bool UseMultiTimeframeConfirmation { get; set; }
        
        [Parameter("Higher Timeframe", DefaultValue = "Hour")]
        public string HigherTimeframeStr { get; set; }
        
        // Indicators
        private RelativeVigorIndex rvi;
        private IchimokuKinkoHyo ichimoku;
        private VolumeWeightedAveragePrice vwap;
        private CustomGoldVolatilityIndex goldVolatility;
        private IntermarketCorrelationIndicator intermarketCorrelation;
        
        // ML Model interface
        private MLModelInterface mlModel;
        
        // Higher timeframe data
        private TimeFrame higherTimeframe;
        private Bars higherTimeframeBars;
        
        // Trading state
        private bool isLongPosition = false;
        private bool isShortPosition = false;
        private double lastTrailingStopLevel = 0;
        
        protected override void OnStart()
        {
            // Parse higher timeframe
            switch (HigherTimeframeStr.ToLower())
            {
                case "minute":
                    higherTimeframe = TimeFrame.Minute;
                    break;
                case "minute5":
                    higherTimeframe = TimeFrame.Minute5;
                    break;
                case "minute15":
                    higherTimeframe = TimeFrame.Minute15;
                    break;
                case "minute30":
                    higherTimeframe = TimeFrame.Minute30;
                    break;
                case "hour":
                    higherTimeframe = TimeFrame.Hour;
                    break;
                case "hour4":
                    higherTimeframe = TimeFrame.Hour4;
                    break;
                case "day":
                    higherTimeframe = TimeFrame.Day;
                    break;
                default:
                    higherTimeframe = TimeFrame.Hour;
                    break;
            }
            
            // Initialize indicators
            rvi = Indicators.RelativeVigorIndex(RviPeriod);
            ichimoku = Indicators.IchimokuKinkoHyo(TenkanPeriod, KijunPeriod, SenkouSpanBPeriod);
            vwap = Indicators.VolumeWeightedAveragePrice(VwapPeriod);
            
            // Initialize custom indicators
            // Note: These are custom indicators that would need to be implemented separately
            goldVolatility = Indicators.GetIndicator<CustomGoldVolatilityIndex>(GoldVolatilityPeriod, 30);
            intermarketCorrelation = Indicators.GetIndicator<IntermarketCorrelationIndicator>();
            
            // Get higher timeframe bars
            higherTimeframeBars = MarketData.GetBars(higherTimeframe);
            
            // Initialize ML model
            mlModel = new MLModelInterface(ModelPath);
            
            // Set up timer for regular model updates (once per day)
            Timer.Start(TimeSpan.FromHours(24), () => {
                mlModel.Retrain(MarketData.GetBars(TimeFrame.Daily, Symbol, 500));
                Print("ML model retrained with latest data");
            });
            
            Print("Gold Trading Strategy initialized successfully");
        }
        
        protected override void OnTick()
        {
            // Update trading state
            UpdatePositionState();
            
            // Process real-time data
            var features = ExtractFeatures();
            
            // Get prediction from ML model
            var prediction = mlModel.Predict(features);
            
            // Execute trading logic based on prediction
            ExecuteTradingLogic(prediction);
            
            // Manage existing positions (trailing stops, etc.)
            ManagePositions();
        }
        
        private void UpdatePositionState()
        {
            // Reset position flags
            isLongPosition = false;
            isShortPosition = false;
            
            // Check current positions
            foreach (var position in Positions)
            {
                if (position.SymbolCode == Symbol.Code)
                {
                    if (position.TradeType == TradeType.Buy)
                    {
                        isLongPosition = true;
                    }
                    else if (position.TradeType == TradeType.Sell)
                    {
                        isShortPosition = true;
                    }
                }
            }
        }
        
        private double[] ExtractFeatures()
        {
            // Extract features from indicators and market data
            // This will feed into our ML model
            return new double[] {
                // Price data
                Bars.Close.Last(0),
                Bars.Open.Last(0),
                Bars.High.Last(0),
                Bars.Low.Last(0),
                Bars.TickVolume.Last(0),
                
                // RVI indicator
                rvi.Main.Last(0),
                rvi.Signal.Last(0),
                
                // Ichimoku indicator
                ichimoku.TenkanSen.Last(0),
                ichimoku.KijunSen.Last(0),
                ichimoku.SenkouSpanA.Last(0),
                ichimoku.SenkouSpanB.Last(0),
                
                // VWAP
                vwap.Result.Last(0),
                
                // Custom indicators
                goldVolatility.Value.Last(0),
                intermarketCorrelation.GoldUsd.Last(0),
                intermarketCorrelation.GoldBonds.Last(0),
                
                // Market context features
                (double)Server.Time.Hour,
                (double)Server.Time.DayOfWeek,
                
                // Higher timeframe data
                higherTimeframeBars.Close.Last(0),
                higherTimeframeBars.Open.Last(0),
                
                // Trend features
                CalculateTrendStrength(5),
                CalculateTrendStrength(10),
                CalculateTrendStrength(20)
            };
        }
        
        private double CalculateTrendStrength(int period)
        {
            // Simple trend strength calculation
            // Positive values indicate uptrend, negative values indicate downtrend
            double sumChanges = 0;
            
            for (int i = 1; i <= period; i++)
            {
                sumChanges += Bars.Close.Last(i-1) - Bars.Close.Last(i);
            }
            
            return sumChanges / period;
        }
        
        private void ExecuteTradingLogic(PredictionResult prediction)
        {
            // Don't trade if we already have a position in the same direction
            if ((prediction.Signal == SignalType.Buy && isLongPosition) ||
                (prediction.Signal == SignalType.Sell && isShortPosition))
            {
                return;
            }
            
            // Check if we should close existing positions in opposite direction
            if (prediction.Signal == SignalType.Buy && isShortPosition)
            {
                ClosePositions(TradeType.Sell);
            }
            else if (prediction.Signal == SignalType.Sell && isLongPosition)
            {
                ClosePositions(TradeType.Buy);
            }
            
            // Execute new trades based on signals with confirmation
            if (prediction.Signal == SignalType.Buy && prediction.Confidence >= 0.6 && ConfirmBuySignal())
            {
                ExecuteBuyOrder();
            }
            else if (prediction.Signal == SignalType.Sell && prediction.Confidence >= 0.6 && ConfirmSellSignal())
            {
                ExecuteSellOrder();
            }
        }
        
        private bool ConfirmBuySignal()
        {
            // Basic confirmation using RVI and Ichimoku
            bool currentTimeframeConfirmation = 
                rvi.Main.Last(0) > rvi.Signal.Last(0) &&
                Bars.Close.Last(0) > ichimoku.TenkanSen.Last(0) &&
                goldVolatility.Value.Last(0) > GoldVolatilityThreshold;
            
            // If multi-timeframe confirmation is not required, return current timeframe result
            if (!UseMultiTimeframeConfirmation)
            {
                return currentTimeframeConfirmation;
            }
            
            // Higher timeframe confirmation
            bool higherTimeframeConfirmation =
                higherTimeframeBars.Close.Last(0) > higherTimeframeBars.Close.Last(1) &&
                higherTimeframeBars.Close.Last(0) > ichimoku.TenkanSen.Last(0);
            
            // Return true only if both timeframes confirm
            return currentTimeframeConfirmation && higherTimeframeConfirmation;
        }
        
        private bool ConfirmSellSignal()
        {
            // Basic confirmation using RVI and Ichimoku
            bool currentTimeframeConfirmation = 
                rvi.Main.Last(0) < rvi.Signal.Last(0) &&
                Bars.Close.Last(0) < ichimoku.TenkanSen.Last(0) &&
                goldVolatility.Value.Last(0) > GoldVolatilityThreshold;
            
            // If multi-timeframe confirmation is not required, return current timeframe result
            if (!UseMultiTimeframeConfirmation)
            {
                return currentTimeframeConfirmation;
            }
            
            // Higher timeframe confirmation
            bool higherTimeframeConfirmation =
                higherTimeframeBars.Close.Last(0) < higherTimeframeBars.Close.Last(1) &&
                higherTimeframeBars.Close.Last(0) < ichimoku.TenkanSen.Last(0);
            
            // Return true only if both timeframes confirm
            return currentTimeframeConfirmation && higherTimeframeConfirmation;
        }
        
        private void ExecuteBuyOrder()
        {
            // Calculate position size based on risk management
            var positionSize = CalculatePositionSize(TradeType.Buy);
            
            // Calculate stop loss and take profit levels
            double stopLossPrice = Symbol.Bid - (StopLossPips * Symbol.PipSize);
            double takeProfitPrice = Symbol.Bid + (TakeProfitPips * Symbol.PipSize);
            
            // Execute buy order
            var result = ExecuteMarketOrder(TradeType.Buy, Symbol, positionSize, "ML Gold Strategy", stopLossPrice, takeProfitPrice);
            
            if (result.IsSuccessful)
            {
                Print($"Buy order executed: {positionSize} lots at {Symbol.Bid}, SL: {stopLossPrice}, TP: {takeProfitPrice}");
                
                // Store initial trailing stop level if using trailing stop
                if (UseTrailingStop)
                {
                    lastTrailingStopLevel = stopLossPrice;
                }
            }
            else
            {
                Print($"Buy order failed: {result.Error}");
            }
        }
        
        private void ExecuteSellOrder()
        {
            // Calculate position size based on risk management
            var positionSize = CalculatePositionSize(TradeType.Sell);
            
            // Calculate stop loss and take profit levels
            double stopLossPrice = Symbol.Ask + (StopLossPips * Symbol.PipSize);
            double takeProfitPrice = Symbol.Ask - (TakeProfitPips * Symbol.PipSize);
            
            // Execute sell order
            var result = ExecuteMarketOrder(TradeType.Sell, Symbol, positionSize, "ML Gold Strategy", stopLossPrice, takeProfitPrice);
            
            if (result.IsSuccessful)
            {
                Print($"Sell order executed: {positionSize} lots at {Symbol.Ask}, SL: {stopLossPrice}, TP: {takeProfitPrice}");
                
                // Store initial trailing stop level if using trailing stop
                if (UseTrailingStop)
                {
                    lastTrailingStopLevel = stopLossPrice;
                }
            }
            else
            {
                Print($"Sell order failed: {result.Error}");
            }
        }
        
        private double CalculatePositionSize(TradeType tradeType)
        {
            // Calculate position size based on account equity and risk percentage
            double riskAmount = Account.Equity * (RiskPercentage / 100.0);
            
            // Calculate pip value
            double pipValue = Symbol.PipValue;
            
            // Calculate position size based on stop loss distance
            double positionSize = riskAmount / (StopLossPips * pipValue);
            
            // Round to standard lot size (0.01 lots)
            positionSize = Math.Floor(positionSize * 100) / 100.0;
            
            // Ensure minimum position size
            if (positionSize < 0.01)
                positionSize = 0.01;
                
            // Ensure position size doesn't exceed account limits
            double maxPositionSize = Account.FreeMargin / (Symbol.MarginPerLot * 2);
            if (positionSize > maxPositionSize)
                positionSize = maxPositionSize;
                
            return positionSize;
        }
        
        private void ClosePositions(TradeType tradeType)
        {
            foreach (var position in Positions)
            {
                if (position.SymbolCode == Symbol.Code && position.TradeType == tradeType)
                {
                    ClosePosition(position);
                    Print($"Closed {tradeType} position: {position.Label}");
                }
            }
        }
        
        private void ManagePositions()
        {
            if (!UseTrailingStop || Positions.Count == 0)
                return;
                
            foreach (var position in Positions)
            {
                if (position.SymbolCode != Symbol.Code)
                    continue;
                    
                // Manage trailing stops
                if (position.TradeType == TradeType.Buy)
                {
                    // Calculate new stop loss level for buy positions
                    double newStopLoss = Symbol.Bid - (TrailingStopDistance * Symbol.PipSize);
                    
                    // Only move stop loss up, never down
                    if (newStopLoss > position.StopLoss && newStopLoss > lastTrailingStopLevel)
                    {
                        ModifyPosition(position, newStopLoss, position.TakeProfit);
                        lastTrailingStopLevel = newStopLoss;
                        Print($"Updated trailing stop for buy position to {newStopLoss}");
                    }
                }
                else if (position.TradeType == TradeType.Sell)
                {
                    // Calculate new stop loss level for sell positions
                    double newStopLoss = Symbol.Ask + (TrailingStopDistance * Symbol.PipSize);
                    
                    // Only move stop loss down, never up
                    if (newStopLoss < position.StopLoss && (lastTrailingStopLevel == 0 || newStopLoss < lastTrailingStopLevel))
                    {
                        ModifyPosition(position, newStopLoss, position.TakeProfit);
                        lastTrailingStopLevel = newStopLoss;
                        Print($"Updated trailing stop for sell position to {newStopLoss}");
                    }
                }
            }
        }
    }
}