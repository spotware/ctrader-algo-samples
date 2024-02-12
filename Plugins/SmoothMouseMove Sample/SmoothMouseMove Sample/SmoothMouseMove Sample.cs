// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample opens a new chart for GPBUSD on start. When the user hovers the mouse cursor over
//    this chart, the plugin shows a Border containing a TextBlock that demonstrates whether showing
//    orders on the chart is enabled/disabled. When the user clicks on the chart, the displaying of
//    orders is enabled/disabled with the text inside the TextBlock being updated automatically.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class SmoothMouseMoveSample : Plugin
    {
        // Declaring the required controls and
        // the variable for storing the Chart
        private TextBlock _ordersStatusTextBlock;
        private Border _ordersStatusBorder;
        private Canvas _canvas;
        private Chart _gpbusdChart;
        
        protected override void OnStart()
        {
            // Opening a new chart for GBPUSD and
            // storing the Chart inside a varaible
            _gpbusdChart = ChartManager.AddChartFrame("GBPUSD", TimeFrame.Minute2).Chart;
            
            // Initialising the Border and
            // setting its parameters
            _ordersStatusBorder = new Border {
                IsHitTestVisible = false,
                CornerRadius = 3,
                BorderThickness = 1,
                Padding = new Thickness(5),
                Margin = new Thickness(5, 5, 0, 0),
                IsVisible = false,            
            };

            // Initialising the TextBlock and
            // settings its initial text
            _ordersStatusTextBlock = new TextBlock
            {
                Text = _gpbusdChart.DisplaySettings.Orders.ToString()
            };

            // Initialising the Canvas and
            // placing the TextBlock inside it
            _canvas = new Canvas();
            _canvas.AddChild(_ordersStatusBorder);
            
            // Placing the TextBlock inside the Border
            _ordersStatusBorder.Child = _ordersStatusTextBlock;

            // Adding the Canvas to the Chart
            _gpbusdChart.AddControl(_canvas);
            
            // Handling various mouse events
            _gpbusdChart.MouseUp += GpbusdChartOnMouseUp;
            _gpbusdChart.MouseEnter += GpbusdChartOnMouseEnter;
            _gpbusdChart.MouseLeave += GpbusdChartOnMouseLeave;
            _gpbusdChart.MouseMove += GpbusdChartOnMouseMove;

        }

        private void GpbusdChartOnMouseMove(ChartMouseEventArgs obj)
        {
            // Moving the border to follow the mouse cursor
            _ordersStatusBorder.Top = obj.MouseY;
            _ordersStatusBorder.Left = obj.MouseX;
        }

        private void GpbusdChartOnMouseLeave(ChartMouseEventArgs obj)
        {
            // Hiding the border whenever the
            // mouse cursor leaves the chart
            _ordersStatusBorder.IsVisible = false;
        }

        private void GpbusdChartOnMouseEnter(ChartMouseEventArgs obj)
        {
            // Showing the border whenever the
            // mouse cursor enters the chart
            _ordersStatusBorder.IsVisible = true;
        }

        private void GpbusdChartOnMouseUp(ChartMouseEventArgs obj)
        {
            // Enabling or disabling the displaying of orders on the chart
            _gpbusdChart.DisplaySettings.Orders = !_gpbusdChart.DisplaySettings.Orders;
            
            // Updating the text inside the TextBlock to match
            // the current settings
            _ordersStatusTextBlock.Text = _gpbusdChart.DisplaySettings.Orders.ToString();
        }
    }        
}