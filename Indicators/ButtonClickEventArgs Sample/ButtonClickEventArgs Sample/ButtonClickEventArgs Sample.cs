using cAlgo.API;

namespace cAlgo
{
    // This example shows how to use the Button object Click event ButtonClickEventArgs
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