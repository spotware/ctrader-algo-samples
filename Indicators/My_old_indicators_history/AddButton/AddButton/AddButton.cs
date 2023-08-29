using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;


namespace ChartControlsTest
{
    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class ChartControls : Indicator
    {
        [Parameter(DefaultValue = "Click Me")]
        public string Text { get; set; }

        [Parameter(DefaultValue = HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [Parameter(DefaultValue = VerticalAlignment.Center)]
        public VerticalAlignment VerticalAlignment { get; set; }

        protected override void Initialize()
        {
            var button = new Button
            {
                Text = Text,
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment
            };

            button.Click += Button_Click;

            Chart.AddControl(button);
        }

        private void Button_Click(ButtonClickEventArgs obj)
        {
            obj.Button.Text = "You clicked me, thanks";
        }

        public override void Calculate(int index)
        {
        }
    }
}