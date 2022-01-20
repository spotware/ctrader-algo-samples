using cAlgo.API;

namespace cAlgo.Robots
{
    // This sample shows how to use PositionModifiedEventArgs
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionModifiedEventArgsSample : Robot
    {
        protected override void OnStart()
        {
            Positions.Modified += Positions_Modified;
        }

        private void Positions_Modified(PositionModifiedEventArgs obj)
        {
            Print("Position {0} modified", obj.Position.Id);
        }
    }
}