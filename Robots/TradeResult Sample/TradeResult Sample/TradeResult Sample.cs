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
    public class TradeResultSample : Robot
    {
        protected override void OnStart()
        {
            var tradeResult = ExecuteMarketOrder(TradeType.Buy, SymbolName, Symbol.VolumeInUnitsMin);

            if (tradeResult.IsSuccessful)
            {
                Print("Market order execution was successful");

                var position = tradeResult.Position;

                Print("A new position opend with ID ", position.Id);
            }
            else
            {
                Print("Market order execution was not successful");
            }
        }
    }
}