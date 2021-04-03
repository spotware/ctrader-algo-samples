using cAlgo.API;

namespace cAlgo
{
    /// <summary>
    /// This sample shows how to use the Radio button chart control
    /// </summary>
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RadioButtonSample : Indicator
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

            var firstRadioButton = new RadioButton
            {
                Text = "Unchecked"
            };

            firstRadioButton.Checked += RadioButton_Checked;
            firstRadioButton.Unchecked += RadioButton_Unchecked;

            stackPanel.AddChild(firstRadioButton);

            var secondRadioButton = new RadioButton
            {
                Text = "Unchecked"
            };

            secondRadioButton.Checked += RadioButton_Checked;
            secondRadioButton.Unchecked += RadioButton_Unchecked;

            stackPanel.AddChild(secondRadioButton);

            Chart.AddControl(stackPanel);
        }

        private void RadioButton_Unchecked(RadioButtonEventArgs obj)
        {
            var radioButton = obj.RadioButton;

            radioButton.Text = "Unchecked";
        }

        private void RadioButton_Checked(RadioButtonEventArgs obj)
        {
            var radioButton = obj.RadioButton;

            radioButton.Text = "Checked";
        }

        public override void Calculate(int index)
        {
        }
    }
}