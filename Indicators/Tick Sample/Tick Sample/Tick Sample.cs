// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TickSample : Indicator
    {
        private Ticks _ticks;

        [Parameter("Symbol Name", DefaultValue = "EURUSD")]
        public string InputSymbolName { get; set; }

        protected override void Initialize()
        {
            // Getting a symbol ticks data
            _ticks = MarketData.GetTicks(InputSymbolName);
            // Subscribing to upcoming ticks
            _ticks.Tick += Ticks_Tick;
        }

        private void Ticks_Tick(TicksTickEventArgs obj)
        {
            // Printing Last tick inside Ticks collection
            Print("Bid: {0} | Ask: {1} | Time: {2:dd/MM/yyyy HH:mm:ss}", obj.Ticks.LastTick.Bid, obj.Ticks.LastTick.Ask, obj.Ticks.LastTick.Time);
        }

        public override void Calculate(int index)
        {
        }
    }
}
