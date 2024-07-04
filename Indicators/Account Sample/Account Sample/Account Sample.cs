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
    public class AccountSample : Indicator
    {
        protected override void Initialize()
        {
            var grid = new Grid(16, 2)
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var style = new Style();

            style.Set(ControlProperty.Padding, 5);
            style.Set(ControlProperty.Margin, 5);
            style.Set(ControlProperty.FontWeight, FontWeight.ExtraBold);
            style.Set(ControlProperty.BackgroundColor, Color.Black);

            grid.AddChild(new TextBlock
            {
                Text = "Account Info",
                Style = style,
                HorizontalAlignment = HorizontalAlignment.Center
            }, 0, 0, 1, 2);

            grid.AddChild(new TextBlock
            {
                Text = "Type",
                Style = style
            }, 1, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.AccountType.ToString(),
                Style = style
            }, 1, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Is Live",
                Style = style
            }, 2, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.IsLive.ToString(),
                Style = style
            }, 2, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Balance",
                Style = style
            }, 3, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.Balance.ToString(),
                Style = style
            }, 3, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Broker Name",
                Style = style
            }, 4, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.BrokerName,
                Style = style
            }, 4, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Currency",
                Style = style
            }, 5, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.Asset.Name,
                Style = style
            }, 5, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Number",
                Style = style
            }, 6, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.Number.ToString(),
                Style = style
            }, 6, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Equity",
                Style = style
            }, 7, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.Equity.ToString(),
                Style = style
            }, 7, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Free Margin",
                Style = style
            }, 8, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.FreeMargin.ToString(),
                Style = style
            }, 8, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Margin",
                Style = style
            }, 9, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.Margin.ToString(),
                Style = style
            }, 9, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Margin Level",
                Style = style
            }, 10, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.MarginLevel.ToString(),
                Style = style
            }, 10, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Precise Leverage",
                Style = style
            }, 11, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.PreciseLeverage.ToString(),
                Style = style
            }, 11, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Stop Out Level",
                Style = style
            }, 12, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.StopOutLevel.ToString(),
                Style = style
            }, 12, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Unrealized Gross Profit",
                Style = style
            }, 13, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.UnrealizedGrossProfit.ToString(),
                Style = style
            }, 13, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Unrealized Net Profit",
                Style = style
            }, 14, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.UnrealizedNetProfit.ToString(),
                Style = style
            }, 14, 1);

            grid.AddChild(new TextBlock
            {
                Text = "User Id",
                Style = style
            }, 15, 0);
            grid.AddChild(new TextBlock
            {
                Text = Account.UserId.ToString(),
                Style = style
            }, 15, 1);

            Chart.AddControl(grid);
        }

        public override void Calculate(int index)
        {
        }
    }
}
