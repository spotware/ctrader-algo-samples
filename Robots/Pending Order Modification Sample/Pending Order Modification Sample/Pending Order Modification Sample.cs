using cAlgo.API;
using System;
using System.Globalization;
using System.Linq;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to modify a pending order
    /// It uses order comment to find the order, you can use order label instead if you want to
    /// Set stop loss and take profit to 0 if you don't want to change it
    /// Leave expiry parameter empty if you don't want to change it or 0 if you want to remove it
    /// If you don't want to change the target price set it to 0
    /// If you don't want to change the volume set it to 0
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PendingOrderModificationSample : Robot
    {
        [Parameter("Order Comment")]
        public string OrderComment { get; set; }

        [Parameter("Order Label")]
        public string OrderLabel { get; set; }

        [Parameter("Target Price", DefaultValue = 0.0)]
        public double TargetPrice { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }

        [Parameter("Stop Loss Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Expiry (HH:mm:ss)")]
        public string Expiry { get; set; }

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Has Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }

        [Parameter("Order Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod OrderTriggerMethod { get; set; }

        [Parameter("Limit Range (Pips)", DefaultValue = 10)]
        public double LimitRangeInPips { get; set; }

        protected override void OnStart()
        {
            PendingOrder order = null;

            if (!string.IsNullOrWhiteSpace(OrderComment) && !string.IsNullOrWhiteSpace(OrderComment))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase) && string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(OrderComment))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, OrderComment, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(OrderLabel))
            {
                order = PendingOrders.FirstOrDefault(iOrder => string.Equals(iOrder.Label, OrderLabel, StringComparison.OrdinalIgnoreCase));
            }

            if (order == null)
            {
                Print("Couldn't find the order, please check the comment and label");

                Stop();
            }

            var targetPrice = TargetPrice == 0 ? order.TargetPrice : TargetPrice;

            var orderSymbol = Symbols.GetSymbol(order.SymbolName);

            var stopLossInPips = StopLossInPips == 0 ? order.StopLossPips : (double?)StopLossInPips;
            var takeProfitInPips = TakeProfitInPips == 0 ? order.TakeProfitPips : (double?)TakeProfitInPips;

            DateTime? expiryTime;

            if (string.IsNullOrWhiteSpace(Expiry))
            {
                expiryTime = order.ExpirationTime;
            }
            else if (Expiry.Equals("0", StringComparison.OrdinalIgnoreCase))
            {
                expiryTime = null;
            }
            else
            {
                var expiryTimeSpan = default(TimeSpan);

                if (!TimeSpan.TryParse(Expiry, CultureInfo.InvariantCulture, out expiryTimeSpan))
                {
                    Print("Your provided value for expiry is not valid, please use HH:mm:ss format");

                    Stop();
                }

                expiryTime = expiryTimeSpan == default(TimeSpan) ? null : (DateTime?)Server.Time.Add(expiryTimeSpan);
            }

            var volumeInUnits = VolumeInLots == 0 ? order.VolumeInUnits : orderSymbol.QuantityToVolumeInUnits(VolumeInLots);

            if (order.OrderType == PendingOrderType.Limit)
            {
                ModifyPendingOrder(order, targetPrice, stopLossInPips, takeProfitInPips, expiryTime, volumeInUnits, HasTrailingStop, StopLossTriggerMethod);
            }
            else if (order.OrderType == PendingOrderType.Stop)
            {
                ModifyPendingOrder(order, targetPrice, stopLossInPips, takeProfitInPips, expiryTime, volumeInUnits, HasTrailingStop, StopLossTriggerMethod, OrderTriggerMethod);
            }
            else if (order.OrderType == PendingOrderType.StopLimit)
            {
                ModifyPendingOrder(order, targetPrice, stopLossInPips, takeProfitInPips, expiryTime, volumeInUnits, HasTrailingStop, StopLossTriggerMethod, OrderTriggerMethod, LimitRangeInPips);
            }
        }
    }
}