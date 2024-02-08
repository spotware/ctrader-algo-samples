// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses the WebSocketClient to connect to a fictional indicator service which outputs
//    text data. The sample parses this text data into a double and uses the result in its own Calculate
//    method to modify the output of a SimpleMovingAverage indicator.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(AccessRights = AccessRights.None)]
    public class WebSocketSample : Indicator
    {
    
        // Defining the indicator output
        [Output("Main")]
        public IndicatorDataSeries Result { get; set; }
        
        // Declaring an additional field to store data from the indicator service
        private double _currentResultFromService;

        // Declaring the required WebSocketClientOptions
        private static WebSocketClientOptions _webSocketClientOptions = new WebSocketClientOptions
        {
            KeepAliveInterval = new TimeSpan(0, 1, 30),
            UseDefaultCredentials = true,
        };
        
        // Initialising the WebSocketClient
        private WebSocketClient _webSocketClient = new WebSocketClient(_webSocketClientOptions);

        // Defining the URI to the fictional indicator service
        private readonly Uri _targetUri = new Uri("ws://bestindicator.io:8000");

        // Initialising the SimpleMovingAverage indicator
        private SimpleMovingAverage _simpleMovingAverage;

        protected override void Initialize()
        {
            // Connecting to the indicator service and handling
            // the TextReceived event
            _webSocketClient.Connect(_targetUri);
            _webSocketClient.TextReceived += IndicatorDataReceived;

            _simpleMovingAverage = Indicators.SimpleMovingAverage(Bars.ClosePrices, 14);
        }
        
        protected void IndicatorDataReceived(WebSocketClientTextReceivedEventArgs obj)
        {
            // On every text received, parse it into a double
            // and store it in a variable
            double.TryParse((string)obj.Text, out _currentResultFromService);
        } 

        public override void Calculate(int index)
        {
            // Modifying the result of the SimplveMovingAverage
            // by multiplying it with the results attained from the
            // indicator service
            Result[index] = _simpleMovingAverage.Result[index] * _currentResultFromService;
        }


    }
}