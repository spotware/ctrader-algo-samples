// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This cBot is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;
using System.Linq;

namespace cAlgo.Robots
{
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