// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ButtonSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold,
                Opacity = 0.7
            };

            for (int i = 0; i < 5; i++)
            {
                var button = new Button
                {
                    Text = "Button #" + i,
                    Margin = 10
                };

                button.Click += Button_Click;

                stackPanel.AddChild(button);
            }

            Chart.AddControl(stackPanel);
        }

        private void Button_Click(ButtonClickEventArgs obj)
        {
            var textSplit = obj.Button.Text.Split(' ').TakeWhile(text => !text.Equals("Clicked", StringComparison.OrdinalIgnoreCase)).ToArray();

            obj.Button.Text = string.Join(" ", textSplit) + " Clicked";
        }

        public override void Calculate(int index)
        {
        }
    }
}
