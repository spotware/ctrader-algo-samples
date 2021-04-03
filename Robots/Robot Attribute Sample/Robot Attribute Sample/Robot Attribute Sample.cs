using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample robot shows how to use the Robot attribute and its properties
    /// Every cBot (Robot) must be annotated with this attribute
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class RobotAttributeSample : Robot
    {
    }
}