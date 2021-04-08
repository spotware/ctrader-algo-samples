using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the TradeType
    /// TradeType is used to set an order trade side or direction
    /// </summary>
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