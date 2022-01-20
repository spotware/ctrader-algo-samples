using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the SymbolTickEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class SymbolTickEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Symbol.Tick += Symbol_Tick;
        }

        private void Symbol_Tick(SymbolTickEventArgs obj)
        {
            Print("Symbol: {0} | Ask: {1} | Bid: {2}", obj.SymbolName, obj.Ask, obj.Bid);
        }

        public override void Calculate(int index)
        {
        }
    }
}