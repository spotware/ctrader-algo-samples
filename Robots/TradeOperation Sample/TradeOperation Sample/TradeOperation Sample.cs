// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TradeOperationSample : Robot
    {
        protected override void OnStart()
        {
            var tradeOperation = ExecuteMarketOrderAsync(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin, OnTradeResult);

            if (tradeOperation.IsExecuting)
            {
                Print("Executing");
            }
            else
            {
                Print("Completed");
            }
        }

        private void OnTradeResult(TradeResult obj)
        {
            Print("Was Trade Operation Successful: ", obj.IsSuccessful);
        }
    }
}