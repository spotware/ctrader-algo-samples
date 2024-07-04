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

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class PositionBarsPassedSample : Robot
    {
        [Parameter("Label", DefaultValue = "MyBot")]
        public string Label { get; set; }

        protected override void OnStart()
        {
            var currentIndex = Bars.Count - 1;

            foreach (var position in Positions)
            {
                if (!string.Equals(position.Label, Label, StringComparison.OrdinalIgnoreCase)) continue;

                var positionOpenBarIndex = Bars.OpenTimes.GetIndexByTime(position.EntryTime);

                var numberOfBarsPassedSincePositionOpened = currentIndex - positionOpenBarIndex;

                Print(numberOfBarsPassedSincePositionOpened);
            }
        }
    }
}