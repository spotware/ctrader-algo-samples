// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses the Chart.MouseMove event to display a custom control that shows the index of
//    the bar over/under which the mouse is hovering. Via the Chart.YToYValue() method, the control
//    also shows the symbol price corresponding to the Y-coordinate of the cursor.
//
// -------------------------------------------------------------------------------------------------

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
    [Indicator(AccessRights = AccessRights.None, IsOverlay = true)]
    public class MouseEvents : Indicator
    {
        // Defining a border inside which information will be displayed
        Border border = new Border
        {
            IsHitTestVisible = false,
            CornerRadius = 3,
            BorderThickness = 1,
            Padding = new Thickness(5),
            Margin = new Thickness(5, 5, 0, 0),
            IsVisible = false,            
        };
        
        // Declaring the text blocks to be shown inside the border
        TextBlock barIndexTB = new TextBlock();
        TextBlock hoverPrice = new TextBlock();

        protected override void Initialize()
        {
            // Setting the container background colour
            border.BackgroundColor = Chart.ColorSettings.BackgroundColor;
            border.BorderColor = Chart.ColorSettings.ForegroundColor;

            // Placing the border inside a Canvas
            var canvas = new Canvas();
            canvas.AddChild(border);
            Chart.AddControl(canvas);

            // Configuring the Grid for placing the text blocks
            var grid = new Grid();
            grid.ShowGridLines = false;
            border.Child = grid;
            grid.AddColumn().SetWidthToAuto();
            grid.AddColumn().SetWidthInPixels(5);
            grid.AddColumn().SetWidthToAuto();

            // Placing the text blocks on different rows of the grid
            // and placing data into the suitable rows and columns
            grid.AddRow();
            grid.AddChild(new TextBlock { Text = "Bar Index" });
            grid.AddChild(barIndexTB, 0, 2);
            
            grid.AddRow();
            grid.AddChild(new TextBlock { Text = "Hover Price"}, 1, 0);
            grid.AddChild(hoverPrice, 1, 2);   
            
            // Assigning custom event handlers to mouve movement events
            Chart.MouseMove += Chart_MouseMove;
            Chart.MouseLeave += Chart_MouseLeave;
            Chart.MouseEnter += Chart_MouseEnter;
        }

        // Showing the custom control when the mouse
        // mouse enters the chart area
        private void Chart_MouseEnter(ChartMouseEventArgs obj)
        {
            border.IsVisible = true;
        }

        // Preventing the custom control from being displayed
        // when the mouse leaves the chart area
        private void Chart_MouseLeave(ChartMouseEventArgs obj)
        {
            border.IsVisible = false;
        }

        private void Chart_MouseMove(ChartMouseEventArgs obj)
        {
            border.IsVisible = true;
            
            // Smoothly moving the custom control to match
            // the position of the mouse cursor
            border.Left = obj.MouseX;
            border.Top = obj.MouseY;
            
            // Updating the text inside the control on any mouse move
            barIndexTB.Text = obj.BarIndex.ToString();
            hoverPrice.Text = Chart.YToYValue(obj.MouseY).ToString();

     }


        public override void Calculate(int index)
        {
        }

    }
}