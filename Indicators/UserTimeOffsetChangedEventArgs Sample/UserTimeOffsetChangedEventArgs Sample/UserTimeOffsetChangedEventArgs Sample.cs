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
    public class UserTimeOffsetChangedEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            Application.UserTimeOffsetChanged += Application_UserTimeOffsetChanged;
        }

        private void Application_UserTimeOffsetChanged(UserTimeOffsetChangedEventArgs obj)
        {
            Print("User time offset changed to: {0}", obj.UserTimeOffset);
        }

        public override void Calculate(int index)
        {
        }
    }
}
