// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This Indicator is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;
using System;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TimeZoneSample : Indicator
    {
        protected override void Initialize()
        {
            // You can get user platform time zone offset like this
            var platformUserSelectedTimeZoneOffset = Application.UserTimeOffset;

            var estTime = GetEasternStandardTime();

            Print(estTime.ToString("o"));
        }

        public override void Calculate(int index)
        {
        }

        private DateTime GetEasternStandardTime()
        {
            var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            return TimeZoneInfo.ConvertTimeFromUtc(Server.TimeInUtc, easternTimeZone);
        }
    }
}
