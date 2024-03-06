// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    During initialisation, this sample opens a new position for the symbol whose chart it is
//    currently attached to. Afterward, the indicator draws a line that measures the percentile
//    distance between the current price of the position and its entry price.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None, IsOverlay = false)]
    public class PositionCurrentPriceSample : Indicator
    {
        // Declaring a variable to store the
        // position that the indicator opens
        private Position _indicatorPosition;
        
        // Declaring the indicator output
        [Output("Result", LineColor = "Orange", PlotType = PlotType.Line)]
        public IndicatorDataSeries Result { get; set; }
        
        protected override void Initialize()
        {
            // Opening a new position on start
            ExecuteMarketOrder(TradeType.Buy, SymbolName, 10000, "indicator sample");
            
            // Storing the new position in the variable
            _indicatorPosition = Positions.Find("indicator sample");
        }

       

        public override void Calculate(int index)
        {
            // Assigning the percentile difference between
            // the entry price and the current price to the
            // indicator output
            Result[index] = ((_indicatorPosition.CurrentPrice - _indicatorPosition.EntryPrice) /
                             _indicatorPosition.EntryPrice) * 100;
        }
    }
}