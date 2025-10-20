// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    All changes to this file might be lost on the next application update.
//    If you are going to modify this file please make a copy using the "Duplicate" command.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using cAlgo.API;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SampleBreakEven : Robot
    {
        private SymbolInfo _symbolInfo;
        private const string DefaultPositionIdParameterValue = "PID";

        [Parameter("Position Id", DefaultValue = DefaultPositionIdParameterValue)]
        public string PositionId { get; set; }

        [Parameter("Add Pips", DefaultValue = 0.0, MinValue = 0.0)]
        public double AddPips { get; set; }

        [Parameter("Trigger Pips", DefaultValue = 10, MinValue = 1)]
        public double TriggerPips { get; set; }

        protected override void OnStart()
        {
            if (PositionId == DefaultPositionIdParameterValue)
                PrintErrorAndStop("You have to specify \"Position Id\" in cBot Parameters");

            if (TriggerPips < AddPips + 2)
                PrintErrorAndStop("\"Trigger Pips\" must be greater or equal to \"Add Pips\" + 2");

            var position = FindPositionOrStop();
            _symbolInfo = Symbols.GetSymbolInfo(position.SymbolName);

            BreakEvenIfNeeded();
        }

        private void PrintErrorAndStop(string errorMessage)
        {
            Print(errorMessage);
            Stop();

            throw new Exception(errorMessage);
        }

        private Position FindPositionOrStop()
        {
            var position = Positions.FirstOrDefault(p => "PID" + p.Id == PositionId || p.Id.ToString() == PositionId);
            if (position == null)
                PrintErrorAndStop("Position with Id = " + PositionId + " doesn't exist");

            return position;
        }

        protected override void OnTick()
        {
            BreakEvenIfNeeded();
        }

        private void BreakEvenIfNeeded()
        {
            var position = FindPositionOrStop();

            if (position.Pips < TriggerPips)
                return;

            var desiredNetProfitInDepositAsset = AddPips * _symbolInfo.PipValue * position.VolumeInUnits;
            var desiredGrossProfitInDepositAsset = desiredNetProfitInDepositAsset - position.Commissions * 2 - position.Swap;
            var quoteToDepositRate = _symbolInfo.PipValue / _symbolInfo.PipSize;
            var priceDifference = desiredGrossProfitInDepositAsset / (position.VolumeInUnits * quoteToDepositRate);

            var priceAdjustment = GetPriceAdjustmentByTradeType(position.TradeType, priceDifference);
            var breakEvenLevel = position.EntryPrice + priceAdjustment;
            var roundedBreakEvenLevel = RoundPrice(breakEvenLevel, position.TradeType);

            ModifyPosition(position, roundedBreakEvenLevel, position.TakeProfit);

            Print("Stop loss for position PID" + position.Id + " has been moved to break even.");
            Print("Stopping cBot..");
            Stop();
        }

        private double RoundPrice(double price, TradeType tradeType)
        {
            var multiplier = Math.Pow(10, _symbolInfo.Digits);

            if (tradeType == TradeType.Buy)
                return Math.Ceiling(price * multiplier) / multiplier;

            return Math.Floor(price * multiplier) / multiplier;
        }

        private static double GetPriceAdjustmentByTradeType(TradeType tradeType, double priceDifference)
        {
            if (tradeType == TradeType.Buy)
                return priceDifference;

            return -priceDifference;
        }
    }
}
