using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use TradeOperation to monitor an async order execution/placement operation
    /// </summary>
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