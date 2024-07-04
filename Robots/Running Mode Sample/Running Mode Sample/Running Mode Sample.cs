// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Robots
{
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