using cAlgo.API;

namespace cAlgo
{
    // This is how you can use the AccessRights property of an Indicator or Robot attribute
    // The default value is None, which means your indicator/cBot will not access anything outside
    // like files, network, windows registry, etc
    // If you try to access any of aforementioned resources you will see an access right error on
    // cTrader automate logs tab if your indicator/cBot access rights were not enough.
    // The FullAccess gives you access to everything, it's like executing a .NET standalone app
    // on a Windows machine.
    // The AccessRights allows cTrader to notify the user of your indicator/cBot that it will
    // access which resources on his system, so he can decide to allow it or not
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
