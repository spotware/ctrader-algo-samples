using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the AccountType
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class AccountTypeSample : Indicator
    {
        protected override void Initialize()
        {
            var text = string.Format("Account Type: {0}", Account.AccountType);

            Chart.DrawStaticText("text", text, VerticalAlignment.Top, HorizontalAlignment.Right, Color.Red);
        }

        public override void Calculate(int index)
        {
        }
    }
}