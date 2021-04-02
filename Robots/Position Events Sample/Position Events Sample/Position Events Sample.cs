using cAlgo.API;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to handle position events
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionEventsSample : Robot
    {
        protected override void OnStart()
        {
            Positions.Opened += Positions_Opened;
            Positions.Closed += Positions_Closed;
            Positions.Modified += Positions_Modified;
        }

        private void Positions_Modified(PositionModifiedEventArgs obj)
        {
            var modifiedPosition = obj.Position;
        }

        private void Positions_Closed(PositionClosedEventArgs obj)
        {
            var closedPosition = obj.Position;

            var closeReason = obj.Reason;
        }

        private void Positions_Opened(PositionOpenedEventArgs obj)
        {
            var openedPosition = obj.Position;
        }
    }
}