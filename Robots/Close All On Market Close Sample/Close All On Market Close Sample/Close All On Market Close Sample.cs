using cAlgo.API;
using System;
using System.Globalization;

namespace cAlgo.Robots
{
    /// <summary>
    /// This cBot closes all open positions before market close
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class CloseAllOnMarketCloseSample : Robot
    {
        private TimeSpan _closeBeforeTime;

        [Parameter("Close Before", DefaultValue = "00:05:00")]
        public string CloseBeforeTime { get; set; }

        protected override void OnStart()
        {
            if (!TimeSpan.TryParse(CloseBeforeTime, CultureInfo.InvariantCulture, out _closeBeforeTime))
            {
                Print("You have provided invalid value for Close Before parameter");

                Stop();
            }

            Timer.Start(1);
        }

        protected override void OnTimer()
        {
            var timeTillClose = Symbol.MarketHours.TimeTillClose();

            if (!Symbol.MarketHours.IsOpened() || timeTillClose > _closeBeforeTime) return;

            foreach (var position in Positions)
            {
                ClosePosition(position);
            }
        }
    }
}