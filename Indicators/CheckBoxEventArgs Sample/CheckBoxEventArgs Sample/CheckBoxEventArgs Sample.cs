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
    public class CheckBoxEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            var checkBox = new CheckBox
            {
                Text = "Check Box",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            checkBox.Click += CheckBox_Click;

            Chart.AddControl(checkBox);
        }

        private void CheckBox_Click(CheckBoxEventArgs obj)
        {
            var state = obj.CheckBox.IsChecked.Value ? "Checked" : "Unchecked";

            obj.CheckBox.Text = state;
        }

        public override void Calculate(int index)
        {
        }
    }
}
