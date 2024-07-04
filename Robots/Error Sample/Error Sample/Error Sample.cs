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