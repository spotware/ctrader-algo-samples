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