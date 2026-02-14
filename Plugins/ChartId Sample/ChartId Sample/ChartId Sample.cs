// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    Using ChartManager, the plugin reacts to new charts being added or removed. On every
//    such action, the plugin prints a message with the log containing the Id of the chart 
//    that has just been added or removed.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
    // Declaring the class as a plugin without requiring special access permissions.
    [Plugin(AccessRights = AccessRights.None)]
    public class ChartIdSample : Plugin
    {
        protected override void OnStart()
        {
            // Assigning custom handlers for the ChartManager.FramesAdded and ChartManager.FramesRemoved events.
            ChartManager.FramesAdded += ChartManager_FramesAdded;
            ChartManager.FramesRemoved += ChartManager_FramesRemoved;
        }

        private void ChartManager_FramesRemoved(FramesRemovedEventArgs obj)
        {
            // Iterating over a collection of removed frames.
            foreach(var frame in obj.RemovedFrames) 
            {
                // Checking if a removed frame is a ChartFrame.
                if (frame is ChartFrame) 
                {
                    // Downcasting the frame to a ChartFrame and printing the message.
                    var chartFrame = frame as ChartFrame;
                    Print($"Chart {chartFrame.Chart.Id} removed");
                }
            }
        }

        private void ChartManager_FramesAdded(FramesAddedEventArgs obj)
        {
            // Iterating over a collection of added frames.
            foreach(var frame in obj.AddedFrames) 
            {
                // Checking if an added frame is a ChartFrame.
                if (frame is ChartFrame) 
                {
                    // Downcasting the frame to a ChartFrame and printing the message.
                    var chartFrame = frame as ChartFrame;
                    Print($"Chart {chartFrame.Chart.Id} added");
                }
            }
        }


    }        
}
