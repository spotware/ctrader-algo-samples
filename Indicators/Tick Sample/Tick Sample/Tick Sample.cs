using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    // This sample indicator shows how to use Tick
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
