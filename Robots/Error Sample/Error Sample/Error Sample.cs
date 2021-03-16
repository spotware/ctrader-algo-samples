using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to get error code of a trade operation when its not successful
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ErrorSample : Robot
    {
        protected override void OnStart()
        {
            // We use 0 for volume to cause an error
            var tradeResult = ExecuteMarketOrder(TradeType.Buy, SymbolName, 0);

            if (!tradeResult.IsSuccessful)
            {
                Print(tradeResult.Error);

                Stop();
            }
        }
    }
}