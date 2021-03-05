using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This indicator shows how to use CharacterCasing on text box controls
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CharacterCasingSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.6,
                Width = 200
            };

            stackPanel.AddChild(new TextBlock
            {
                Text = "Lower Character Casing",
                Margin = new Thickness(10, 10, 10, 0),
                ForegroundColor = Color.Red,
                FontWeight = FontWeight.ExtraBold
            });

            stackPanel.AddChild(new TextBox { CharacterCasing = CharacterCasing.Lower, Margin = 10 });

            stackPanel.AddChild(new TextBlock
            {
                Text = "Upper Character Casing",
                Margin = new Thickness(10, 10, 10, 0),
                ForegroundColor = Color.Red,
                FontWeight = FontWeight.ExtraBold
            });

            stackPanel.AddChild(new TextBox { CharacterCasing = CharacterCasing.Upper, Margin = 10 });

            stackPanel.AddChild(new TextBlock
            {
                Text = "Normal Character Casing",
                Margin = new Thickness(10, 10, 10, 0),
                ForegroundColor = Color.Red,
                FontWeight = FontWeight.ExtraBold
            });

            stackPanel.AddChild(new TextBox { CharacterCasing = CharacterCasing.Normal, Margin = 10 });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}