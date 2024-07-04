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
using System.Linq;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionModificationSample : Robot
    {
        [Parameter("Position Comment")]
        public string PositionComment { get; set; }

        [Parameter("Position Label")]
        public string PositionLabel { get; set; }

        [Parameter("Stop Loss (Pips)", DefaultValue = 10)]
        public double StopLossInPips { get; set; }

        [Parameter("Stop Loss Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; }

        [Parameter("Take Profit (Pips)", DefaultValue = 10)]
        public double TakeProfitInPips { get; set; }

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Has Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }

        protected override void OnStart()
        {
            Position position = null;

            if (!string.IsNullOrWhiteSpace(PositionComment) && !string.IsNullOrWhiteSpace(PositionLabel))
            {
                position = Positions.FindAll(PositionLabel).FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(PositionComment))
            {
                position = Positions.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(PositionLabel))
            {
                position = Positions.Find(PositionLabel);
            }

            if (position == null)
            {
                Print("Couldn't find the position, please check the comment and label");

                Stop();
            }

            var positionSymbol = Symbols.GetSymbol(position.SymbolName);

            var stopLossInPrice = position.StopLoss;

            if (StopLossInPips > 0)
            {
                var stopLossInPipsPrice = StopLossInPips * positionSymbol.PipSize;

                stopLossInPrice = position.TradeType == TradeType.Buy ? position.EntryPrice - stopLossInPipsPrice : position.EntryPrice + stopLossInPipsPrice;
            }

            var takeProfitInPrice = position.TakeProfit;

            if (TakeProfitInPips > 0)
            {
                var takeProfitInPipsPrice = TakeProfitInPips * positionSymbol.PipSize;

                takeProfitInPrice = position.TradeType == TradeType.Buy ? position.EntryPrice + takeProfitInPipsPrice : position.EntryPrice - takeProfitInPipsPrice;
            }

            ModifyPosition(position, stopLossInPrice, takeProfitInPrice, HasTrailingStop, StopLossTriggerMethod);

            if (VolumeInLots > 0)
            {
                var volumeInUnits = positionSymbol.QuantityToVolumeInUnits(VolumeInLots);

                ModifyPosition(position, volumeInUnits);
            }
        }
    }
}