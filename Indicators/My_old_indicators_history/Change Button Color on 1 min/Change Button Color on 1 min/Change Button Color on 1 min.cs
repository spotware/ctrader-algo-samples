using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class ChangeButtonColoron1min : Indicator
    {

        [Parameter(DefaultValue = "Click Me")]
        public string Text { get; set; }

        [Parameter(DefaultValue = HorizontalAlignment.Center)]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [Parameter(DefaultValue = VerticalAlignment.Center)]
        public VerticalAlignment VerticalAlignment { get; set; }

        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }


        private Button button;
        private bool ColorChange = true;

        protected override void Initialize()
        {
            button = new Button
            {
                Text = Text,
                HorizontalAlignment = HorizontalAlignment,
                VerticalAlignment = VerticalAlignment,
                BackgroundColor = Color.Red
            };

            Chart.AddControl(button);
        }

        public override void Calculate(int index)
        {
            /*if (IsLastBar && ColorChange)
            {
                button.BackgroundColor = Color.Green;
                ColorChange = false;
            }
            else
            {
                button.BackgroundColor = Color.Red;
                ColorChange = true;
            }*/
        }

    }
}