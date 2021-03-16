using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows the use of FillRule
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FillRuleSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanelNonzero = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
            };

            stackPanelNonzero.AddChild(new TextBlock { Text = "Nonzero", ForegroundColor = Color.Black, HorizontalAlignment = HorizontalAlignment.Center, Margin = 10 });

            stackPanelNonzero.AddChild(new Polygon
            {
                FillColor = Color.Red,
                Width = 200,
                Height = 100,
                FillRule = FillRule.Nonzero,
                Margin = 10,
                Points = new Point[]
                {
                    new Point(1, 200),
                    new Point(50, 30),
                    new Point(100, 1),
                    new Point(150, 1),
                    new Point(100, 10),
                    new Point(50, 1),
                    new Point(200, 70),
                    new Point(300, 90),
                }
            });

            stackPanelNonzero.AddChild(new TextBlock { Text = "EvenOdd", ForegroundColor = Color.Black, HorizontalAlignment = HorizontalAlignment.Center, Margin = 10 });

            stackPanelNonzero.AddChild(new Polygon
            {
                FillColor = Color.Red,
                Width = 200,
                Height = 100,
                FillRule = FillRule.EvenOdd,
                Margin = 10,
                Points = new Point[]
                {
                    new Point(1, 200),
                    new Point(50, 30),
                    new Point(100, 1),
                    new Point(150, 1),
                    new Point(100, 10),
                    new Point(50, 1),
                    new Point(200, 70),
                    new Point(300, 90),
                }
            });

            Chart.AddControl(stackPanelNonzero);
        }

        public override void Calculate(int index)
        {
        }
    }
}