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
    public class Windows1 : Indicator
    {
        private double height;
        private double width;
        private Window window;
        
        protected override void Initialize()
        {
            window = new Window
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
            
             var scrollViewer = new ScrollViewer
             {
                 HorizontalAlignment = HorizontalAlignment.Center,
                 VerticalAlignment = VerticalAlignment.Center,
                 BackgroundColor = Color.Gold,
                 Opacity = 0.7,
                 HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                 VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                 Height = 100
             };
             
            
            window.Show();
            //Chart.AddControl(scrollViewer);
            //window.Child(scrollViewer); направление в целом
            //window.AddControl(scrollViewer);
            window.SizeChanged += window_SizeChanged;
            height = window.Height;
            width = window.Width;                
        }

        private void window_SizeChanged(WindowSizeChangedEventArgs obj)
        {
            //throw new NotImplementedException();
            double height = obj.Window.Height;
            double width = obj.Window.Width;
            Print("Height: {0}, Width: {1}", height, width);
            Print("Height: {0}, Width: {1}", window.Height , window.Width);
     
        }
        
        

        public override void Calculate(int index)
        {
        }
    }
}