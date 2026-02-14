// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to execute multiple market orders asynchronously, 
//    then waits for all orders to complete before closing the positions. 
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System.Linq;
using System.Threading;

namespace RefreshDataSample
{
    // Define the cBot attributes, such as AccessRights.
    [Robot(AccessRights = AccessRights.None)]
    public class RefreshDataSample : Robot
    {
        // This method is called when the cBot starts and is used for initialisation.
        protected override void OnStart()
        {
            // Initialise an array to store 50 trade operations.
            var execututionResults = new TradeOperation[50];

            // Loop to send 50 market orders, alternating between buy and sell orders.
            for (var i = 0; i < 50; i++)
            {
                // Send an asynchronous market order, alternating between buy and sell based on the index.
                // A buy order is sent for even indices, a sell order is sent for odd ones, with the minimum volume in units.
                execututionResults[i] = ExecuteMarketOrderAsync(i % 2 == 0 ? TradeType.Buy : TradeType.Sell, SymbolName, Symbol.VolumeInUnitsMin);
            }

            Print("All orders sent");  // Print message once all orders are sent.

            // Wait for all orders to finish execution.
            while (execututionResults.Any(operation => operation.IsExecuting))
            {
                Print("Waiting...");  // Print message while waiting for orders to complete.
                Thread.Sleep(100);  // Pause the execution for 100 milliseconds.
                
                // Refresh the data to ensure the cBot can continue executing after the sleep period.
                // If you remove the RefreshData method call cBot main thread will stuck and the rest of the code will not be executed.
                RefreshData();
            }

            Print("Closing Positions");  // Print message once all orders are completed.

            // Iterate over all open positions to close them, but skip the sell positions.
            foreach (var position in Positions)
            {
                if (position.TradeType == TradeType.Sell) continue;  // Skip closing sell positions.

                _ = ClosePositionAsync(position);  // Asynchronously close each position that is not a sell order.
            }
        }
    }
}
