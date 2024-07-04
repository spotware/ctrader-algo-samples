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
    public class GridRowSample : Indicator
    {
        [Parameter("Grid Rows #", DefaultValue = 10)]
        public int GridRowsNumber { get; set; }

        [Parameter("Grid Row Length", DefaultValue = 2)]
        public int GridRowLength { get; set; }

        [Parameter("Grid Row Length Unit Type", DefaultValue = GridUnitType.Auto)]
        public GridUnitType GridRowLengthUnitType { get; set; }

        protected override void Initialize()
        {
            var grid = new Grid(GridRowsNumber, 2)
            {
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true
            };
            for (int iRow = 0; iRow < GridRowsNumber; iRow++)
            {
                var row = grid.Rows[iRow];

                SetGridRowLength(row);

                for (int iColumn = 0; iColumn < 2; iColumn++)
                {
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

        public override void Calculate(int index)
        {
        }
    }
}
