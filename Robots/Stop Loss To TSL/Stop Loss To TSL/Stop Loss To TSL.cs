using cAlgo.API;
using System;

namespace cAlgo.Robots
{
    /// <summary>
    /// Changes current symbol positions normal stop loss to trailing stop loss once it reached x amount of pips in profit
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class StopLossToTSL : Robot
    {
        [Parameter("Pips Profit", DefaultValue = 20)]
        public double PipsProfit { get; set; }

        protected override void OnTick()
        {
            foreach (var position in Positions)
            {
                if (!position.SymbolName.Equals(SymbolName, StringComparison.OrdinalIgnoreCase)
                    || position.HasTrailingStop
                    || !position.StopLoss.HasValue
                    || position.Pips < PipsProfit) continue;

                ModifyPosition(position, position.StopLoss, position.TakeProfit, true, position.StopLossTriggerMethod);
            }
        }
    }
}