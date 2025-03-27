// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This sample cBot adds and starts two other cBots based on a certain logic. 
//
//    For a detailed tutorial on creating this cBot, see this video: https://www.youtube.com/watch?v=DUzdEt30OSE
//
// -------------------------------------------------------------------------------------------------


using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as AccessRights and its ability to add indicators.
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class AddcBots : Robot
    {
        // Hold references to the two cBots being managed by this robot.
        ChartRobot _robot1;
        ChartRobot _robot2;

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Add two instances of "Sample Trend cBot" with unique parameters to the chart.
            _robot1 = Chart.Robots.Add("Sample Trend cBot", 0.01, MovingAverageType.Simple, Bars.ClosePrices, 10, 5);
            _robot2 = Chart.Robots.Add("Sample Trend cBot", 0.01, MovingAverageType.Simple, Bars.ClosePrices, 12, 7);

            // Subscribe to the event triggered when a robot starts.
            Chart.Robots.RobotStarted += ChartRobots_RobotStarted;
        }

        // Event handler related to the started robot.
        private void ChartRobots_RobotStarted(ChartRobotStartedEventArgs obj)
        {
            Print ("Robot Started");  // Logs a message indicating that a robot has started.
        }

        // This method is triggered whenever a bar is closed.
        protected override void OnBarClosed()
        {
            // Check if the first robot is stopped and starts it while stopping the second robot.
            if (_robot1.State == RobotState.Stopped)
            {
                _robot1.Start();  // Starts the first robot.
                _robot2.Stop();  // Stops the second robot.
            }

            // Check if the first robot is running and stops it while starting the second robot.
            else if (_robot1.State == RobotState.Running)
            {
                _robot2.Start();  // Starts the second robot.
                _robot1.Stop();  // Stops the first robot.
            }
        }

        // This method is called when the cBot is stopped.
        protected override void OnStop()
        {
            // Handle cBot stop here.
        }
    }
}
