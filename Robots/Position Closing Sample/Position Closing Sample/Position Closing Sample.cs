using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to close a position
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionClosingSample : Robot
    {
        [Parameter("Position Comment")]
        public string PositionComment { get; set; }

        [Parameter("Position Label")]
        public string PositionLabel { get; set; }

        protected override void OnStart()
        {
            Position position = null;

            if (!string.IsNullOrWhiteSpace(PositionComment) && !string.IsNullOrWhiteSpace(PositionLabel))
            {
                position = Positions.FindAll(PositionLabel).FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(PositionComment))
            {
                position = Positions.FirstOrDefault(iOrder => string.Equals(iOrder.Comment, PositionComment, StringComparison.OrdinalIgnoreCase));
            }
            else if (!string.IsNullOrWhiteSpace(PositionLabel))
            {
                position = Positions.Find(PositionLabel);
            }

            if (position == null)
            {
                Print("Couldn't find the position, please check the comment and label");

                Stop();
            }

            ClosePosition(position);
        }
    }
}