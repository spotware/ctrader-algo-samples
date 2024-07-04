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
    public class TextTrimmingSample : Indicator
    {
        [Parameter("Text", DefaultValue = "very long texttttttttttttttttttttt")]
        public string Text { get; set; }

        [Parameter("Trimming", DefaultValue = TextTrimming.CharacterEllipsis)]
        public TextTrimming TextTrimming { get; set; }

        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                BackgroundColor = Color.Gold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Opacity = 0.6,
                Width = 100
            };

            stackPanel.AddChild(new TextBlock
            {
                Text = Text,
                FontWeight = FontWeight.ExtraBold,
                ForegroundColor = Color.Blue,
                TextTrimming = TextTrimming
            });

            Chart.AddControl(stackPanel);
        }

        public override void Calculate(int index)
        {
        }
    }
}
