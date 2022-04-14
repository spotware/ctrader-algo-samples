using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the RadioButtonEventArgs
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RadioButtonEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            var firstRadioButton = new RadioButton 
            {
                Text = "First Radio Button"
            };

            firstRadioButton.Checked += RadioButton_Changed;
            firstRadioButton.Unchecked += RadioButton_Changed;

            var secondRadioButton = new RadioButton 
            {
                Text = "Second Radio Button"
            };

            secondRadioButton.Checked += RadioButton_Changed;
            secondRadioButton.Unchecked += RadioButton_Changed;

            var panel = new StackPanel 
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            panel.AddChild(firstRadioButton);
            panel.AddChild(secondRadioButton);

            Chart.AddControl(panel);
        }

        private void RadioButton_Changed(RadioButtonEventArgs obj)
        {
            var state = obj.RadioButton.IsChecked ? "Checked" : "Unchecked";

            obj.RadioButton.Text = state;
        }

        public override void Calculate(int index)
        {
        }
    }
}
