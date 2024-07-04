// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class HistorySample : Indicator
    {
        private Style _textBlocksStyle;
        private StackPanel _stackPanel;
        private Grid _tradesGrid;

        protected override void Initialize()
        {
            _stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold
            };
            _textBlocksStyle = new Style();
            _textBlocksStyle.Set(ControlProperty.Margin, 5);
            _stackPanel.AddChild(new TextBox
            {
                Text = "Your Last 10 Trades",
                FontWeight = FontWeight.ExtraBold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Style = _textBlocksStyle
            });

            Chart.AddControl(_stackPanel);

            UpdateLastTradeTextBlock();

            Positions.Closed += args => UpdateLastTradeTextBlock();
        }

        public override void Calculate(int index)
        {
        }

        private void UpdateLastTradeTextBlock()
        {
            if (_tradesGrid != null)
                _stackPanel.RemoveChild(_tradesGrid);
            _tradesGrid = new Grid(11, 6);
            _tradesGrid.AddChild(new TextBlock
            {
                Text = "Symbol",
                Style = _textBlocksStyle
            }, 0, 0);
            _tradesGrid.AddChild(new TextBlock
            {
                Text = "Direction",
                Style = _textBlocksStyle
            }, 0, 1);
            _tradesGrid.AddChild(new TextBlock
            {
                Text = "Volume",
                Style = _textBlocksStyle
            }, 0, 2);
            _tradesGrid.AddChild(new TextBlock
            {
                Text = "Open Time",
                Style = _textBlocksStyle
            }, 0, 3);
            _tradesGrid.AddChild(new TextBlock
            {
                Text = "Close Time",
                Style = _textBlocksStyle
            }, 0, 4);
            _tradesGrid.AddChild(new TextBlock
            {
                Text = "Net Profit",
                Style = _textBlocksStyle
            }, 0, 5);

            var lastTenTrades = History.OrderByDescending(iTrade => iTrade.ClosingTime).Take(10).ToArray();

            for (int iRowIndex = 1; iRowIndex <= lastTenTrades.Length; iRowIndex++)
            {
                var trade = lastTenTrades[iRowIndex - 1];
                _tradesGrid.AddChild(new TextBlock
                {
                    Text = trade.SymbolName,
                    Style = _textBlocksStyle
                }, iRowIndex, 0);
                _tradesGrid.AddChild(new TextBlock
                {
                    Text = trade.TradeType.ToString(),
                    Style = _textBlocksStyle
                }, iRowIndex, 1);
                _tradesGrid.AddChild(new TextBlock
                {
                    Text = trade.VolumeInUnits.ToString(),
                    Style = _textBlocksStyle
                }, iRowIndex, 2);
                _tradesGrid.AddChild(new TextBlock
                {
                    Text = trade.EntryTime.ToString("g"),
                    Style = _textBlocksStyle
                }, iRowIndex, 3);
                _tradesGrid.AddChild(new TextBlock
                {
                    Text = trade.ClosingTime.ToString("g"),
                    Style = _textBlocksStyle
                }, iRowIndex, 4);
                _tradesGrid.AddChild(new TextBlock
                {
                    Text = trade.NetProfit.ToString(),
                    Style = _textBlocksStyle
                }, iRowIndex, 5);
            }
            _stackPanel.AddChild(_tradesGrid);
        }
    }
}
