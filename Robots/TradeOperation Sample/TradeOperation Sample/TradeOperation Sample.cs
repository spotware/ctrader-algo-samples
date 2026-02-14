// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates the use of asynchronous trade execution using the ExecuteMarketOrderAsync 
//    method. It executes a buy order asynchronously and checks whether the order was successfully placed 
//    or not. The outcome is printed in the log based on the execution result.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TradeOperationSample : Robot
    {
        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Asynchronously execute a market order for a buy trade.
            var tradeOperation = ExecuteMarketOrderAsync(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin, OnTradeResult);

            // Check if the trade operation is currently executing.
            if (tradeOperation.IsExecuting)
            {
                Print("Executing");  // Print message indicating the trade is still executing.
            }
            
            // Check if the trade operation is not executing, which means it is completed.
            else
            {
                Print("Completed");  // Print message indicating the trade operation has completed.
            }
        }

        // This method is triggered upon trade operation result.
        private void OnTradeResult(TradeResult obj)
        {
            Print("Was Trade Operation Successful: ", obj.IsSuccessful);  // Print whether the trade operation was successful or not.
        }
    }
}
