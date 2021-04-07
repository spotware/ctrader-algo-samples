using cAlgo.API;
using System;

namespace cAlgo
{
    /// <summary>
    /// This sample indicator shows how to work with time zones
    /// Every new cBot/Indicator default time zone is set to UTC via Indicator/Robot attributes TimeZone property
    /// To change it you can set the attribute time zone property value to any of supported time zones
    /// For example:
    /// [Indicator(IsOverlay = true, TimeZone = TimeZones.EasternStandardTime, AccessRights = AccessRights.None)]
    /// You can also work with different time zones programmatically by using .NET libraries\
    /// </summary>
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