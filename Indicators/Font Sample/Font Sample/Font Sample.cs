using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use a chart control font properties to set font size, style, and family
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class FontSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel 
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.6
            };

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Thin Weight Size 10 FontStyle Normal Font Default",
                FontSize = 10,
                FontWeight = FontWeight.Thin,
                FontStyle = FontStyle.Normal,
                ForegroundColor = Color.Black,
                Margin = 10
            });

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Thin Weight Size 10 FontStyle Italic Font Default",
                FontSize = 10,
                FontWeight = FontWeight.Thin,
                FontStyle = FontStyle.Italic,
                ForegroundColor = Color.Black,
                Margin = 10
            });

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Thin Weight Size 10 FontStyle Oblique Font Default",
                FontSize = 10,
                FontWeight = FontWeight.Thin,
                FontStyle = FontStyle.Oblique,
                ForegroundColor = Color.Black,
                Margin = 10
            });

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Black Weight Size 10 FontStyle Normal Font Default",
                FontSize = 10,
                FontWeight = FontWeight.Black,
                FontStyle = FontStyle.Normal,
                ForegroundColor = Color.Black,
                Margin = 10
            });

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Bold Weight Size 10 FontStyle Normal Font Default",
                FontSize = 10,
                FontWeight = FontWeight.Bold,
                FontStyle = FontStyle.Normal,
                ForegroundColor = Color.Black,
                Margin = 10
            });

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Heavy Weight Size 10 FontStyle Normal Font Default",
                FontSize = 10,
                FontWeight = FontWeight.Heavy,
                FontStyle = FontStyle.Normal,
                ForegroundColor = Color.Black,
                Margin = 10
            });

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Bold Weight Size 12 FontStyle Normal Font Default",
                FontSize = 12,
                FontWeight = FontWeight.Bold,
                FontStyle = FontStyle.Normal,
                ForegroundColor = Color.Black,
                Margin = 10
            });

            stackPanel.AddChild(new TextBlock 
            {
                Text = "Thin Weight Size 12 FontStyle Normal Font Calibri Light Italic",
                FontSize = 12,
                FontWeight = FontWeight.Thin,
                FontStyle = FontStyle.Normal,
                ForegroundColor = Color.Black,
                FontFamily = "Calibri Light Italic",
                Margin = 10
            });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
