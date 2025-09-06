// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    This cBot demonstrates how to execute a market order with a specified trade type (buy or sell). 
//    The trade type is passed as a parameter and the cBot executes a market order with the minimum volume
//    allowed by the symbol using that trade type.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as TimeZone and AccessRights.
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TradeTypeSample : Robot
    {
        [Parameter("Trade Type", DefaultValue = TradeType.Buy)]
        public TradeType TradeType { get; set; }  // Define a parameter for the trade type, the default value is set to 'Buy'.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            ExecuteMarketOrder(TradeType, SymbolName, Symbol.VolumeInUnitsMin);  // Executes a market order with the specified trade type, using the symbol minimum volume.
        }
    }
}
