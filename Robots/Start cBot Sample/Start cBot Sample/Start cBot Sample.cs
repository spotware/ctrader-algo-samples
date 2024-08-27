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
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class AddcBots : Robot
    {

        ChartRobot _robot1;
        ChartRobot _robot2;

        protected override void OnStart()
        {
            _robot1 = Chart.Robots.Add("Sample Trend cBot", 0.01, MovingAverageType.Simple, Bars.ClosePrices, 10, 5);
            _robot2 = Chart.Robots.Add("Sample Trend cBot", 0.01, MovingAverageType.Simple, Bars.ClosePrices, 12, 7);

            Chart.Robots.RobotStarted += ChartRobots_RobotStarted;
        }

        private void ChartRobots_RobotStarted(ChartRobotStartedEventArgs obj)
        {
            Print ("Robot Started");

        }

        protected override void OnBarClosed()
        {
            if (_robot1.State == RobotState.Stopped)
            {
                _robot1.Start();
                _robot2.Stop();
            }

            else if (_robot1.State == RobotState.Running)
            {
                _robot2.Start();
                _robot1.Stop();
            }
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}