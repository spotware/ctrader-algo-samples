using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to derive your cBot class from main Robot base class
    /// Every cBot (Robot) must be derived from Robot base class
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RobotSample : Robot
    {
        protected override void OnStart()
        {
            // Put your initialization logic here
        }

        protected override void OnTick()
        {
            // Put your core logic here for new ticks of current chart symbol
        }

        protected override void OnBar()
        {
            // Put your core logic here for new bars of current chart symbol
        }

        protected override void OnTimer()
        {
            // Put your logic here for timer if you started it, otherwise this method will not be called
        }

        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }
    }
}