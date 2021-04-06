using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use control style to change a group of controls style instead of setting each control properties separatly
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class StyleSample : Indicator
    {
        protected override void Initialize()
        {
            var style = new Style();

            style.Set(ControlProperty.Margin, 5);
            style.Set(ControlProperty.ForegroundColor, Color.Blue);
            style.Set(ControlProperty.FontSize, 14);
            style.Set(ControlProperty.Width, 100);

            var stackPanel = new StackPanel 
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Orientation = Orientation.Vertical
            };

            for (var i = 0; i < 10; i++)
            {
                stackPanel.AddChild(new TextBlock 
                {
                    Text = "Textr Block #" + i,
                    Style = style
                });
            }

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
