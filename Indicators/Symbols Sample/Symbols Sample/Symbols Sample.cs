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
    public class SymbolsSample : Indicator
    {
        protected override void Initialize()
        {
            var scrollViewer = new ScrollViewer
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.7,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                Height = 300
            };

            var grid = new Grid(Symbols.Count, 2)
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            scrollViewer.Content = grid;

            for (int iSymbol = 0; iSymbol < Symbols.Count; iSymbol++)
            {
                var symbolName = Symbols[iSymbol];
                var symbol = Symbols.GetSymbol(symbolName);

                if (!symbol.MarketHours.IsOpened())
                    continue;

                grid.AddChild(new TextBlock
                {
                    Text = symbolName,
                    Margin = 5,
                    ForegroundColor = Color.Black,
                    FontWeight = FontWeight.ExtraBold
                }, iSymbol, 0);

                grid.AddChild(new Button
                {
                    Text = symbol.Description,
                    Margin = 5,
                    ForegroundColor = Color.Black,
                    FontWeight = FontWeight.ExtraBold
                }, iSymbol, 1);
            }

            Chart.AddControl(scrollViewer);
        }

        public override void Calculate(int index)
        {
        }
    }
}
