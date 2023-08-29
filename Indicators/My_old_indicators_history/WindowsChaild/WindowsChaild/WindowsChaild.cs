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
    public class WindowSample : Indicator
    {
        
        protected override void Initialize()
        {
            var window = new Window
            {
                Child = new TextBlock 
                {
                    Text = "Hi, This is my Window!",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 20,
                    FontWeight = FontWeight.UltraBold
                },

                Title = "My Window",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Topmost = true
            };
            
            var newTextBlock = new TextBlock
            {
                Text = "111"
            };
            
                       
        }

        public override void Calculate(int index)
        {
        }
    }
}
