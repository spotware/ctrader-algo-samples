// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds an Active Symbol Panel (ASP) tab and uses ChartRobots API to show stats about
//    active chart cBots and lets you add and remove cBots to active chart.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    // Declare the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class ChartRobotsSample : Plugin
    {
        private ChartRobotsControl _chartRobotsControl;
        
        // This method is executed when the plugin starts.
        protected override void OnStart()
        {
            var aspTab = Asp.AddTab("Chart Robots");  // Add a new tab to the ASP named "Chart Robots".
            
            // Create an instance of ChartRobotsControl and set its properties.
            _chartRobotsControl = new ChartRobotsControl(AlgoRegistry)
            {
                VerticalAlignment = VerticalAlignment.Top  // Align the control at the top of the panel.
            };

            // Assign the created ChartRobotsControl as the child control of the ASP tab.
            aspTab.Child = _chartRobotsControl;

            // Call the method to set the chart for the control.
            SetControlChart();

            // Subscribe to the ActiveFrameChanged event to update the chart whenever the active chart frame changes.
            ChartManager.ActiveFrameChanged += _ => SetControlChart();
        }

        // This method sets the chart for the ChartRobotsControl based on the active chart frame.
        private void SetControlChart()
        {
            // Check if the active frame is a ChartFrame.
            if (ChartManager.ActiveFrame is not ChartFrame chartFrame)
                return;

            // Set the chart property of the ChartRobotsControl to the active chart from the ChartFrame.
            _chartRobotsControl.Chart = chartFrame.Chart;
        }
    }        
}
