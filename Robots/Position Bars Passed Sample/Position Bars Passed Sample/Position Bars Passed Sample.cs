using cAlgo.API;
using System;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample shows how to get a position entery time bar index and calculate the number of bars passed since its entery time
    /// </summary>
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