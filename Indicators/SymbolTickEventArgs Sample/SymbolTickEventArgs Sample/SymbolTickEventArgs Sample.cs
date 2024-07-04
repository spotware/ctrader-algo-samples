// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo
{
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
