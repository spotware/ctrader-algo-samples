// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to execute a market order and check if the order was successful or not.
//    It opens a buy position using the minimum volume allowed by the symbol and prints the result based on
//    whether the order was successful. If successful, it prints the position ID of the newly opened position.
//    If unsuccessful, it prints a failure message.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TradeResultSample : Robot
    {
        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Execute a market order for a buy trade, with the minimum trade volume and returns the trade result.
            var tradeResult = ExecuteMarketOrder(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin);

            // Check whether the market order execution was successful.
            if (tradeResult.IsSuccessful)
            {
                Print("Market order execution was successful");  // Print a success message indicating the market order was executed successfully.

                var position = tradeResult.Position;  // Store the position object from the trade result.

                Print("A new position opend with ID ", position.Id);  // Print the ID of the newly opened position.
            }
            
            // If the trade result indicates that the order was not successful.
            else
            {
                Print("Market order execution was not successful");  // Print a failure message indicating the market order execution was not successful.
            }
        }
    }
}
