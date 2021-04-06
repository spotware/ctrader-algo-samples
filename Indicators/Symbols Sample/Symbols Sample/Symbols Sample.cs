using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use Symbols collection to get symbols data
    /// </summary>
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

            var grid = new Grid(Symbols.Count + 1, 2)
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            scrollViewer.Content = grid;

            grid.AddChild(new TextBlock
            {
                Text = "Name",
                Margin = 5,
                ForegroundColor = Color.Black,
                FontWeight = FontWeight.ExtraBold
            }, 0, 0);

            grid.AddChild(new TextBlock
            {
                Text = "Description",
                Margin = 5,
                ForegroundColor = Color.Black,
                FontWeight = FontWeight.ExtraBold
            }, 0, 1);

            for (int iSymbol = 1; iSymbol < Symbols.Count + 1; iSymbol++)
            {
                var symbolName = Symbols[iSymbol];
                var symbol = Symbols.GetSymbol(symbolName);

                if (!symbol.MarketHours.IsOpened()) continue;

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