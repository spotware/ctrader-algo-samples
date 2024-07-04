// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using cAlgo.API.Internals;
using System;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TradingSessionSample : Indicator
    {
        private TextBlock _isOpenedTextBlock;

        private TextBlock _timeTillCloseTextBlock;

        private TextBlock _timeTillOpenTextBlock;

        private Symbol _symbol;

        [Parameter("Use Current Symbol", DefaultValue = true)]
        public bool UseCurrentSymbol { get; set; }

        [Parameter("Other Symbol Name", DefaultValue = "GBPUSD")]
        public string OtherSymbolName { get; set; }

        protected override void Initialize()
        {
            var grid = new Grid(6, 2)
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            var style = new Style();

            style.Set(ControlProperty.Padding, 1);
            style.Set(ControlProperty.Margin, 2);
            style.Set(ControlProperty.BackgroundColor, Color.Black);
            style.Set(ControlProperty.FontSize, 8);

            _symbol = UseCurrentSymbol ? Symbol : Symbols.GetSymbol(OtherSymbolName);

            grid.AddChild(new TextBlock
            {
                Text = "Symbol Info",
                Style = style,
                HorizontalAlignment = HorizontalAlignment.Center
            }, 0, 0, 1, 2);

            grid.AddChild(new TextBlock
            {
                Text = "Time Till Open",
                Style = style
            }, 1, 0);

            _timeTillOpenTextBlock = new TextBlock
            {
                Text = _symbol.MarketHours.TimeTillOpen().ToString(),
                Style = style
            };

            grid.AddChild(_timeTillOpenTextBlock, 1, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Time Till Close",
                Style = style
            }, 2, 0);

            _timeTillCloseTextBlock = new TextBlock
            {
                Text = _symbol.MarketHours.TimeTillClose().ToString(),
                Style = style
            };

            grid.AddChild(_timeTillCloseTextBlock, 2, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Is Opened",
                Style = style
            }, 3, 0);

            _isOpenedTextBlock = new TextBlock
            {
                Text = _symbol.MarketHours.IsOpened().ToString(),
                Style = style
            };

            grid.AddChild(_isOpenedTextBlock, 3, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Trading Sessions #",
                Style = style
            }, 4, 0);

            grid.AddChild(new TextBlock
            {
                Text = _symbol.MarketHours.Sessions.Count.ToString(),
                Style = style
            }, 4, 1);

            grid.AddChild(new TextBlock
            {
                Text = "Trading Session Week Days",
                Style = style
            }, 5, 0);

            var weekDays = string.Empty;

            for (var iSession = 0; iSession < _symbol.MarketHours.Sessions.Count; iSession++)
            {
                var currentSessionWeekDays = string.Format("{0}({1})-{2}({3})", _symbol.MarketHours.Sessions[iSession].StartDay, _symbol.MarketHours.Sessions[iSession].StartTime, _symbol.MarketHours.Sessions[iSession].EndDay, _symbol.MarketHours.Sessions[iSession].EndTime);
                weekDays = iSession == 0 ? currentSessionWeekDays : string.Format("{0}, {1}", weekDays, currentSessionWeekDays);
            }

            grid.AddChild(new TextBlock
            {
                Text = weekDays,
                Style = style
            }, 5, 1);

            Chart.AddControl(grid);

            Timer.Start(TimeSpan.FromSeconds(1));
        }

        protected override void OnTimer()
        {
            _timeTillOpenTextBlock.Text = _symbol.MarketHours.TimeTillOpen().ToString();
            _timeTillCloseTextBlock.Text = _symbol.MarketHours.TimeTillClose().ToString();
            _isOpenedTextBlock.Text = _symbol.MarketHours.IsOpened().ToString();
        }

        public override void Calculate(int index)
        {
        }
    }
}
