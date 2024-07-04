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
    public class TradeTypeSample : Robot
    {
        [Parameter("Trade Type", DefaultValue = TradeType.Buy)]
        public TradeType TradeType { get; set; }

        protected override void OnStart()
        {
            ExecuteMarketOrder(TradeType, SymbolName, Symbol.VolumeInUnitsMin);
        }
    }
}