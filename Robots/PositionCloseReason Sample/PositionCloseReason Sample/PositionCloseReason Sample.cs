using cAlgo.API;

namespace cAlgo.Robots
{
    // This sample shows how to use PositionCloseReason
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionCloseReasonSample : Robot
    {
        protected override void OnStart()
        {
            Positions.Closed += Positions_Closed;
        }

        private void Positions_Closed(PositionClosedEventArgs obj)
        {
            Print(obj.Reason);

            switch (obj.Reason)
            {
                case PositionCloseReason.Closed:
                    // Do something if position closed
                    break;

                case PositionCloseReason.StopLoss:
                    // Do something if position stop loss got hit
                    break;

                case PositionCloseReason.StopOut:
                    // Do something if position stopped out
                    break;

                case PositionCloseReason.TakeProfit:
                    // Do something if position take profit got hit
                    break;
            }
        }
    }
}