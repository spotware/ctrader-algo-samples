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
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public class AccessRightSample : Indicator
    {
        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        {
        }
    }
}
