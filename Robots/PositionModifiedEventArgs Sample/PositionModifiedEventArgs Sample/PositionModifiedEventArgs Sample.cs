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