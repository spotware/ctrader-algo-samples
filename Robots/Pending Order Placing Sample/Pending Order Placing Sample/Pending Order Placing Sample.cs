// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Globalization;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderPlacingSample : Robot
    {
        [Parameter("Type", DefaultValue = PendingOrderType.Limit)]
        public PendingOrderType OrderType { get; set; }

        [Parameter("Direction", DefaultValue = TradeType.Buy)]
        public TradeType OrderTradeType { get; set; }

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Distance (Pips)", DefaultValue = 20, MinValue = 1)]
        public double DistanceInPips { get; set; }

        [Parameter("Stop (Pips)", DefaultValue = 10, MinValue = 0)]
        public double StopInPips { get; set; }

        [Parameter("Target (Pips)", DefaultValue = 10, MinValue = 0)]
        public double TargetInPips { get; set; }

        [Parameter("Limit Range (Pips)", DefaultValue = 10, MinValue = 1)]
        public double LimitRangeInPips { get; set; }

        [Parameter("Expiry", DefaultValue = "00:00:00")]
        public string Expiry { get; set; }

        [Parameter("Label")]
        public string Label { get; set; }

        [Parameter("Comment")]
        public string Comment { get; set; }

        [Parameter("Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }

        [Parameter("Stop Loss Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; }

        [Parameter("Stop Order Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopOrderTriggerMethod { get; set; }

        [Parameter("Async", DefaultValue = false)]
        public bool IsAsync { get; set; }

        protected override void OnStart()
        {
            var volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            DistanceInPips *= Symbol.PipSize;

            var stopLoss = StopInPips == 0 ? null : (double?)StopInPips;
            var takeProfit = TargetInPips == 0 ? null : (double?)TargetInPips;

            TimeSpan expiry;

            if (!TimeSpan.TryParse(Expiry, CultureInfo.InvariantCulture, out expiry))
            {
                Print("Invalid expiry");

                Stop();
            }

            var expiryTime = expiry != TimeSpan.FromSeconds(0) ? (DateTime?)Server.Time.Add(expiry) : null;

            TradeResult result = null;

            switch (OrderType)
            {
                case PendingOrderType.Limit:
                    var limitPrice = OrderTradeType == TradeType.Buy ? Symbol.Ask - DistanceInPips : Symbol.Ask + DistanceInPips;

                    if (IsAsync)
                        PlaceLimitOrderAsync(OrderTradeType, SymbolName, volumeInUnits, limitPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, OnCompleted);
                    else
                        result = PlaceLimitOrder(OrderTradeType, SymbolName, volumeInUnits, limitPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod);

                    break;

                case PendingOrderType.Stop:
                    var stopPrice = OrderTradeType == TradeType.Buy ? Symbol.Ask + DistanceInPips : Symbol.Ask - DistanceInPips;

                    if (IsAsync)
                        PlaceStopOrderAsync(OrderTradeType, SymbolName, volumeInUnits, stopPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod, OnCompleted);
                    else
                        result = PlaceStopOrder(OrderTradeType, SymbolName, volumeInUnits, stopPrice, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod);

                    break;

                case PendingOrderType.StopLimit:
                    var stopLimitPrice = OrderTradeType == TradeType.Buy ? Symbol.Ask + DistanceInPips : Symbol.Ask - DistanceInPips;

                    if (IsAsync)
                        PlaceStopLimitOrderAsync(OrderTradeType, SymbolName, volumeInUnits, stopLimitPrice, LimitRangeInPips, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod, OnCompleted);
                    else
                        result = PlaceStopLimitOrder(OrderTradeType, SymbolName, volumeInUnits, stopLimitPrice, LimitRangeInPips, Label, stopLoss, takeProfit, expiryTime, Comment, HasTrailingStop, StopLossTriggerMethod, StopOrderTriggerMethod);

                    break;

                default:
                    Print("Invalid order type");

                    throw new ArgumentOutOfRangeException("OrderType");
            }

            if (!IsAsync) OnCompleted(result);
        }

        private void OnCompleted(TradeResult result)
        {
            if (!result.IsSuccessful) Print("Error: ", result.Error);

            Stop();
        }
    }
}