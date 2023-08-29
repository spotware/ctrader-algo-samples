using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class ToggleButtonSample : Indicator
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
                var toggleButton = new ToggleButton 
                {
                    Text = "Toggle Button #" + i + " Unchecked",
                    Margin = 10
                };

                toggleButton.Checked += ToggleButton_Checked;
                toggleButton.Unchecked += ToggleButton_Unchecked;

                stackPanel.AddChild(toggleButton);
            }

            Chart.AddControl(stackPanel);
        }

        private void ToggleButton_Checked(ToggleButtonEventArgs obj)
        {
            var textSplit = obj.ToggleButton.Text.Split(' ').TakeWhile(text => !text.Equals("Unchecked", StringComparison.OrdinalIgnoreCase)).ToArray();

            obj.ToggleButton.Text = string.Join(" ", textSplit) + " Checked";
        }

        private void ToggleButton_Unchecked(ToggleButtonEventArgs obj)
        {
            var textSplit = obj.ToggleButton.Text.Split(' ').TakeWhile(text => !text.Equals("Checked", StringComparison.OrdinalIgnoreCase)).ToArray();

            obj.ToggleButton.Text = string.Join(" ", textSplit) + " Unchecked";
        }

        public override void Calculate(int index)
        {
        }
    }
}
