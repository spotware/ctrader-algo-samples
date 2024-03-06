// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample adds two simple moving averages to the chart to which it is attached. The sample
//    uses the ADX indicator to determine the line colour of these two moving averages. If ADX > 30,
//    the trend is strong, and the SMA lines are green and blue. If ADX <= 30, the trend is week, and
//    the SMA lines are orange and purple.
//
// -------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class ChartIndicatorsSample : Indicator
    {
        // Defining the indicators to be added to the chart
        ChartIndicator _saSlow;
        ChartIndicator _saFast;
        
        // Defining the indicator to extract the ADX value from
        private AverageDirectionalMovementIndexRating _averageDirectionalMovementIndexRating;
        
        
        protected override void Initialize()
        {
            // Adding the two SMAs on indicator start
            _saSlow = ChartIndicators.Add("Simple Moving Average", "Close", 50);
            _saFast = ChartIndicators.Add("Simple Moving Average", "Close", 20);
            
            // Initialising the indicator from which the ADX value will be taken
            _averageDirectionalMovementIndexRating = Indicators.AverageDirectionalMovementIndexRating(20);
            
        }

        public override void Calculate(int index)
        {
            // Accessing and comparing the ADX value to 30
            if (_averageDirectionalMovementIndexRating.ADX[index] > 30) 
            {
            
                // Setting the SMA line properties
                _saFast.Lines[0].Color = Color.Green;
                _saFast.Lines[0].Thickness = 3;
                _saSlow.Lines[0].Color = Color.Blue;
                _saSlow.Lines[0].Thickness = 3;
            } else 
            {
            
                // Setting the SMA line properties
                _saFast.Lines[0].Color = Color.Orange;
                _saFast.Lines[0].Thickness = 3;
                _saSlow.Lines[0].Color = Color.Purple;
                _saSlow.Lines[0].Thickness = 3;
            }
        }
    }
}