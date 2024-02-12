// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds a single Border that follows the mouse cursor on the chart to which the cBot
//    is attached. The TextBlock inside the Border shows whether showing the grid is enabled in the
//    chart display settings. On mouse click, the cBot toggles grid display on/off, which also triggers
//    the text inside the TextBlock to update.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class SmoothMouseMoveSample : Robot
    {

        // Declaring the required UI elements
        private Border _gridStatusBorder;
        private TextBlock _gridStatusTextBlock;
        private Canvas _canvas;

        protected override void OnStart()
        {
            // Initialising the Border and setting
            // its parameters
            _gridStatusBorder = new Border {
                IsHitTestVisible = false,
                CornerRadius = 3,
                BorderThickness = 5,
                Padding = new Thickness(5),
                Margin = new Thickness(5, 5, 0, 0),
                BackgroundColor = Color.Aqua,
                MaxHeight = 50,
                MaxWidth = 50,
            };

            // Initialising the TextBlock and making it so
            // it displays whether grid display is on/off
            _gridStatusTextBlock = new TextBlock
            {
                Text = Chart.DisplaySettings.Grid.ToString()
            };

            // Placing controls inside one another
            // and adding the Canvas to the Chart
            _gridStatusBorder.Child = _gridStatusTextBlock;
            _canvas = new Canvas();
            _canvas.AddChild(_gridStatusBorder);
            Chart.AddControl(_canvas);
            
            // Handling various mouse events
            Chart.MouseUp += ChartOnMouseUp;
            Chart.MouseEnter += ChartOnMouseEnter;
            Chart.MouseMove += ChartOnMouseMove;
            Chart.MouseLeave += ChartOnMouseLeave;
        }

        private void ChartOnMouseLeave(ChartMouseEventArgs obj)
        {
            // Hiding the border whenever the mouse cursor
            // leaves the chart
            _gridStatusBorder.IsVisible = false;
        }

        private void ChartOnMouseMove(ChartMouseEventArgs obj)
        {
            // Moving the Border to match the position
            // of the mouse cursor
            _gridStatusBorder.Top = obj.MouseY;
            _gridStatusBorder.Left = obj.MouseX;
        }

        private void ChartOnMouseEnter(ChartMouseEventArgs obj)
        {
            // Start displaying the border whenever
            // the mouse cursor enters the chart
            _gridStatusBorder.IsVisible = true;
        }

        private void ChartOnMouseUp(ChartMouseEventArgs obj)
        {
            // Setting the grid display on/off
            // on mouse click
            Chart.DisplaySettings.Grid = !Chart.DisplaySettings.Grid;
            
            // Updating the text inside the TextBlock
            _gridStatusTextBlock.Text = Chart.DisplaySettings.Grid.ToString();
        }
    }
}