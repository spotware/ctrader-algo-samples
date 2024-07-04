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
    public class CheckBoxControlSample : Indicator
    {
        protected override void Initialize()
        {
            var stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Gold
            };

            var checkBox = new CheckBox
            {
                Text = "Unchecked",
                Margin = 10,
                FontWeight = FontWeight.ExtraBold
            };

            checkBox.Checked += CheckBox_Checked;
            checkBox.Unchecked += CheckBox_Unchecked;

            stackPanel.AddChild(checkBox);

            Chart.AddControl(stackPanel);
        }

        private void CheckBox_Unchecked(CheckBoxEventArgs obj)
        {
            obj.CheckBox.Text = "Unchecked";
        }

        private void CheckBox_Checked(CheckBoxEventArgs obj)
        {
            obj.CheckBox.Text = "Checked";
        }

        public override void Calculate(int index)
        {
        }
    }
}
