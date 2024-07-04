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
    public class ButtonClickEventArgsSample : Indicator
    {
        protected override void Initialize()
        {
            var button = new Button
            {
                Text = "Button not clicked yet",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Whenever you click on the button the Button_Click method will be called
            button.Click += Button_Click;

            Chart.AddControl(button);
        }

        // Here we change the butto test when it's clicked
        private void Button_Click(ButtonClickEventArgs obj)
        {
            obj.Button.Text = "Button Clicked";
        }

        public override void Calculate(int index)
        {
        }
    }
}
