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
