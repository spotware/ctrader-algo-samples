// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample draws a border inside the chart to which the indicator is attached. The TextBlock
//    inside the chart displays the index of the bar over which the mouse cursor is currently hovering,
//    which is achieved via the ChartArea.XToBarIndex() method.
//
// -------------------------------------------------------------------------------------------------


using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class CoordinatesConversionSample : Indicator
    {

        // Declaring the required control
        private Border _border;
        private TextBlock _barIndexTextBlock;
        private Canvas _canvas;

        protected override void Initialize()
        {
            // Initialising the border and setting
            // its parameters
            _border = new Border
            {
                BorderColor = Color.Gray,
                BackgroundColor = Color.Gold,
                BorderThickness = new Thickness(3, 3, 3, 3),
                Padding = new Thickness(5, 5, 5 , 5),
                CornerRadius = new CornerRadius(5),
                Width = 80,
                Height = 40,
                
            };
            
            // Initialising the TextBlock and setting
            // its parameters
            _barIndexTextBlock = new TextBlock
            {
                Text = "...",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            
            // Initialising the Canvas where
            // the Border will be placed
            _canvas = new Canvas();
            
            // Adding controls as children
            _border.Child = _barIndexTextBlock;
            _canvas.AddChild(_border);
            
            // Handling various mouse events
            Chart.MouseLeave += ChartOnMouseLeave;
            Chart.MouseEnter += ChartOnMouseEnter;
            Chart.MouseMove += ChartOnMouseMove;
            
            // Showing the Canvas on the instance chart
            Chart.AddControl(_canvas);
            
        }

        private void ChartOnMouseMove(ChartMouseEventArgs obj)
        {
            // Moving the border together with the mouse cursor
            _border.Left = obj.MouseX;
            _border.Top = obj.MouseY;
            
            // Updating the text inside the TextBlock when
            // the mouse cursor moves
            _barIndexTextBlock.Text = Chart.XToBarIndex(obj.MouseX).ToString();
        }

        private void ChartOnMouseLeave(ChartMouseEventArgs obj)
        {
            // Hiding the border when the cursor leaves
            // the chart
            _border.IsVisible = false;
        }

        private void ChartOnMouseEnter(ChartMouseEventArgs obj)
        {
            // Showing the border when the cursor enters
            // the chart
            _border.IsVisible = true;
        }

        public override void Calculate(int index)
        {
        }
    }
}