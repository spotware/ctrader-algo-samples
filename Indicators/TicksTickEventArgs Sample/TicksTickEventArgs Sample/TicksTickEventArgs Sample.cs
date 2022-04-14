using cAlgo.API;
using cAlgo.API.Internals;

namespace cAlgo
{
    // This sample indicator shows how to use TicksTickEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TicksTickEventArgsSample : Indicator
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
            Print(obj.Ticks.LastTick);
        }

        public override void Calculate(int index)
        {
        }
    }
}
