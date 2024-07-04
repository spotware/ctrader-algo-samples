// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System.Linq;
using System.Threading;

namespace RefreshDataSample
{
    [Robot(AccessRights = AccessRights.None)]
    public class RefreshDataSample : Robot
    {
        protected override void OnStart()
        {
            var execututionResults = new TradeOperation[50];

            for (var i = 0; i < 50; i++)
            {
                execututionResults[i] = ExecuteMarketOrderAsync(i % 2 == 0 ? TradeType.Buy : TradeType.Sell, SymbolName, Symbol.VolumeInUnitsMin);
            }

            Print("All orders sent");

            while (execututionResults.Any(operation => operation.IsExecuting))
            {
                Print("Waiting...");
                Thread.Sleep(100);
                // If you remove the RefreshData method call
                // cBot main thread will stuck and the rest
                // of the code will not be executed
                RefreshData();
            }

            Print("Closing Positions");

            foreach (var position in Positions)
            {
                if (position.TradeType == TradeType.Sell) continue;

                _ = ClosePositionAsync(position);
            }
        }
    }
}