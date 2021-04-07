using cAlgo.API;
using System;

namespace cAlgo.Robots
{
    /// <summary>
    /// This sample cBot shows how to use the API Timer, this timer works both on live and back test
    /// The timer is available for both cBots and indicators
    /// Every cBot/indicator can have one single timer
    /// You can also use the .NET timers if you want to but those will not work properly on back test
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TimerSample : Robot
    {
        protected override void OnStart()
        {
            // You can start the cBot timer by calling Timer.Start method, for interval you can pass a time span or seconds
            Timer.Start(TimeSpan.FromSeconds(1));
            // You can also use the TimerTick event instead of OnTimer method
            Timer.TimerTick += Timer_TimerTick;

            // To stop the timer you can call Timer.Stop method anywhere on your cBot/indicator
            Timer.Stop();
        }

        private void Timer_TimerTick()
        {
            // Put your logic for timer elapsed event here
        }

        protected override void OnTimer()
        {
            // Put your logic for timer elapsed event here
        }
    }
}