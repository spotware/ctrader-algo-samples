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
    public class GridSample : Indicator
    {
        [Parameter("Grid Rows #", DefaultValue = 10)]
        public int GridRowsNumber { get; set; }
        [Parameter("Grid Columns #", DefaultValue = 2)]
        public int GridColumnsNumber { get; set; }
        [Parameter("Grid Row Length", DefaultValue = 2)]
        public int GridRowLength { get; set; }
        [Parameter("Grid Row Length Unit Type", DefaultValue = GridUnitType.Auto)]
        public GridUnitType GridRowLengthUnitType { get; set; }
        [Parameter("Grid Column Length", DefaultValue = 2)]
        public int GridColumnLength { get; set; }
        [Parameter("Grid Column Length Unit Type", DefaultValue = GridUnitType.Auto)]
        public GridUnitType GridColumnLengthUnitType { get; set; }
        protected override void Initialize()
        {
            var grid = new Grid(GridRowsNumber, GridColumnsNumber)
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true,
            };
            for (int iRow = 0; iRow < GridRowsNumber; iRow++)
            {
                var row = grid.Rows[iRow];
                SetGridRowLength(row);
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
        private void SetGridRowLength(GridRow row)
        {
            switch (GridRowLengthUnitType)
            {
                case GridUnitType.Auto:
                    row.SetHeightToAuto();
                    break;
                case GridUnitType.Pixel:
                    row.SetHeightInPixels(GridRowLength);
                    break;
                case GridUnitType.Star:
                    row.SetHeightInStars(GridRowLength);
                    break;
            }
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
