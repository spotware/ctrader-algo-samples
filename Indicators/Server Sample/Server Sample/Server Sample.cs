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
    public class ServerSample : Indicator
    {
        private TextBlock _isConnectedTextBlock;

        protected override void Initialize()
        {
            var grid = new Grid(4, 2)
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
                Text = "Server Info",
                Style = style,
                HorizontalAlignment = HorizontalAlignment.Center
            }, 0, 0, 1, 2);

            grid.AddChild(new TextBlock
            {
                Text = "Time",
                Style = style
            }, 1, 0);
            grid.AddChild(new TextBlock
            {
                Text = Server.Time.ToString("o"),
                Style = style
            }, 1, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Time (UTC)",
                Style = style
            }, 2, 0);
            grid.AddChild(new TextBlock
            {
                Text = Server.TimeInUtc.ToString("o"),
                Style = style
            }, 2, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Is Connected",
                Style = style
            }, 3, 0);

            _isConnectedTextBlock = new TextBlock
            {
                Text = Server.IsConnected ? "Yes" : "No",
                Style = style
            };

            Server.Connected += () => _isConnectedTextBlock.Text = "Yes";
            Server.Disconnected += () => _isConnectedTextBlock.Text = "No";

            grid.AddChild(_isConnectedTextBlock, 3, 1);

            Chart.AddControl(grid);
        }

        public override void Calculate(int index)
        {
        }
    }
}
