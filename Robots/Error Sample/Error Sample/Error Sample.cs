// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    In this example, a market order is placed with a volume of 0, which will result in an error.
//    The bot checks if the trade was successful and, if not, prints the error message and stops.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot with the UTC time zone and no access rights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ErrorSample : Robot
    {
        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Attempt to place a market order with a volume of 0, which will result in an error.
            var tradeResult = ExecuteMarketOrder(TradeType.Buy, SymbolName, 0);

            // If the trade is not successful, print the error message and stop the bot.
            if (!tradeResult.IsSuccessful)
            {
                Print(tradeResult.Error);  // Print the error to the log.

                Stop();  // Stop the bot as there was an error.
            }
        }
    }
}
