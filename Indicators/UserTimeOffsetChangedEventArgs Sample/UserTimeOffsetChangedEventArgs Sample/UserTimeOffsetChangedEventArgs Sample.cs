using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the UserTimeOffsetChangedEventArgs
    // Change your cTrader platform time offset/zone to see the print message
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