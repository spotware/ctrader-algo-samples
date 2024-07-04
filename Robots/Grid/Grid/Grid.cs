// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None)]
    public class Grid : Robot
    {
        [Parameter("Volume (lots)", DefaultValue = 0.01, MinValue = 0.01, Step = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Trade Side")]
        public TradeType TradeType { get; set; }

        [Parameter("Step (pips)", DefaultValue = 5, MinValue = 0.1, Step = 0.1)]
        public double StepPips { get; set; }

        [Parameter("Target Profit", DefaultValue = 20)]
        public double TargetProfit { get; set; }

        private bool enoughMoney = true;

        protected override void OnStart()
        {
            if (GridPositions.Length == 0)
                OpenPosition();
        }

        protected override void OnTick()
        {
            if (GridPositions.Sum(p => p.NetProfit) >= TargetProfit)
            {
                Print("Target profit is reached. Closing all grid positions");
                CloseGridPositions();
                Print("All grid positions are closed. Stopping cBot");
                Stop();
            }
            if (GridPositions.Length > 0 && enoughMoney)
            {
                var lastGridPosition = GridPositions.OrderBy(p => p.Pips).Last();
                var distance = CalculateDistanceInPips(lastGridPosition);

                if (distance >= StepPips)
                    OpenPosition();
            }
        }

        private Position[] GridPositions
        {
            get
            {
                return Positions
                    .Where(p => p.SymbolName == SymbolName && p.TradeType == TradeType)
                    .ToArray();
            }
        }

        private double CalculateDistanceInPips(Position position)
        {
            if (position.TradeType == TradeType.Buy)
                return (position.EntryPrice - Symbol.Ask) / Symbol.PipSize;
            else
                return (Symbol.Bid - position.EntryPrice) / Symbol.PipSize;
        }

        private void OpenPosition()
        {
            var result = ExecuteMarketOrder(TradeType, SymbolName, Symbol.QuantityToVolumeInUnits(VolumeInLots), "Grid");
            if (result.Error == ErrorCode.NoMoney)
            {
                enoughMoney = false;
                Print("Not enough money to open additional positions");
            }
        }

        private void CloseGridPositions()
        {
            while (GridPositions.Length > 0)
            {
                foreach (var position in GridPositions)
                {
                    ClosePosition(position);
                }
            }
        }
    }
}