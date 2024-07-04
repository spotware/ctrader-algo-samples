// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionExecutionSample : Robot
    {
        [Parameter("Direction", DefaultValue = TradeType.Buy)]
        public TradeType Direction { get; set; }

        [Parameter("Volume (Lots)", DefaultValue = 0.01)]
        public double VolumeInLots { get; set; }

        [Parameter("Distance (Pips)", DefaultValue = 20, MinValue = 1)]
        public double DistanceInPips { get; set; }

        [Parameter("Stop (Pips)", DefaultValue = 10, MinValue = 0)]
        public double StopInPips { get; set; }

        [Parameter("Target (Pips)", DefaultValue = 10, MinValue = 0)]
        public double TargetInPips { get; set; }

        [Parameter("Label")]
        public string Label { get; set; }

        [Parameter("Comment")]
        public string Comment { get; set; }

        [Parameter("Trailing Stop", DefaultValue = false)]
        public bool HasTrailingStop { get; set; }

        [Parameter("Stop Loss Trigger Method", DefaultValue = StopTriggerMethod.Trade)]
        public StopTriggerMethod StopLossTriggerMethod { get; set; }

        [Parameter("Async", DefaultValue = false)]
        public bool IsAsync { get; set; }

        protected override void OnStart()
        {
            var volumeInUnits = Symbol.QuantityToVolumeInUnits(VolumeInLots);

            DistanceInPips *= Symbol.PipSize;

            var stopLoss = StopInPips == 0 ? null : (double?)StopInPips;
            var takeProfit = TargetInPips == 0 ? null : (double?)TargetInPips;

            TradeResult result = null;

            if (IsAsync)
                ExecuteMarketOrderAsync(Direction, SymbolName, volumeInUnits, Label, stopLoss, takeProfit, Comment, HasTrailingStop, StopLossTriggerMethod, OnCompleted);
            else
                result = ExecuteMarketOrder(Direction, SymbolName, volumeInUnits, Label, stopLoss, takeProfit, Comment, HasTrailingStop, StopLossTriggerMethod);

            if (!IsAsync) OnCompleted(result);
        }

        private void OnCompleted(TradeResult result)
        {
            if (!result.IsSuccessful) Print("Error: ", result.Error);

            Stop();
        }
    }
}