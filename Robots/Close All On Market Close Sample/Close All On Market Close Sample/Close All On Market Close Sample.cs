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
using System.Globalization;

namespace cAlgo.Robots
{
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