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
    public class GridColumnSample : Indicator
    {
        [Parameter("Grid Columns #", DefaultValue = 2)]
        public int GridColumnsNumber { get; set; }

        [Parameter("Grid Column Length", DefaultValue = 2)]
        public int GridColumnLength { get; set; }

        [Parameter("Grid Column Length Unit Type", DefaultValue = GridUnitType.Auto)]
        public GridUnitType GridColumnLengthUnitType { get; set; }

        protected override void Initialize()
        {
            var grid = new Grid(5, GridColumnsNumber)
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true
            };
            for (int iRow = 0; iRow < 5; iRow++)
            {
                for (int iColumn = 0; iColumn < GridColumnsNumber; iColumn++)
                {
                    var column = grid.Columns[iColumn];
                    SetGridColumnLength(column);
                    grid.AddChild(new TextBlock
                    {
                        Text = string.Format("Row {0} and Column {1}", iRow, iColumn),
                        Margin = 5,
                        ForegroundColor = Color.Black,
                        FontWeight = FontWeight.ExtraBold
                    }, iRow, iColumn);
                }
            }

            Chart.AddControl(grid);
        }

        private void SetGridColumnLength(GridColumn column)
        {
            switch (GridColumnLengthUnitType)
            {
                case GridUnitType.Auto:
                    column.SetWidthToAuto();
                    break;

                case GridUnitType.Pixel:
                    column.SetWidthInPixels(GridColumnLength);
                    break;

                case GridUnitType.Star:
                    column.SetWidthInStars(GridColumnLength);
                    break;
            }
        }

        public override void Calculate(int index)
        {
        }
    }
}
