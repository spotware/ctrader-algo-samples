using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to use the RunningMode property of your robot
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RunningModeSample : Robot
    {
        protected override void OnStart()
        {
            switch (RunningMode)
            {
                case RunningMode.RealTime:
                    // If the robot is running on real time market condition
                    break;

                case RunningMode.SilentBacktesting:
                    // If the robot is running on backtest and the visual mode is off
                    break;

                case RunningMode.VisualBacktesting:
                    // If the robot is running on backtest and the visual mode is on
                    break;

                case RunningMode.Optimization:
                    // If the robot is running on optimizer
                    break;
            }
        }
    }
}