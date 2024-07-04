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