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