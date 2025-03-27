// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    The code is provided as a sample only and does not guarantee any particular outcome or profit of any kind. Use it at your own risk.
//    
//    This sample cBot subscribes to a symbol price feed and streams the prices.   
//
//    For a detailed tutorial on creating this cBot, see this video: https://www.youtube.com/watch?v=y5ARwEEXSLI
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    // Define the cBot attributes, such as AccessRights and its ability to add indicators.
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class WebSocketsExample : Robot
    {
        private WebSocketClient _webSocketClient = new WebSocketClient();  // Initialise a WebSocket client for managing server communication.
        private readonly Uri _targetUri = new Uri("wss://marketdata.tradermade.com/feedadv");  // Define the WebSocket server URI for connecting to the price feed.

        // This method is called when the cBot starts.
        protected override void OnStart()
        {
            // Initiate a connection to the specified WebSocket server.
            _webSocketClient.Connect(_targetUri);

            // Subscribe to the TextReceived event to handle incoming messages.
            _webSocketClient.TextReceived += _webSocketClient_TextReceived;

            // Create a JSON payload with authentication and subscription details.
            var data = "{\"userKey\":\"PasteStreamingKeyHere\", \"symbol\":\"EURUSD\"}";

            // Send the subscription request to the WebSocket server.
            _webSocketClient.Send(data);
        }

        // Event handler for processing received WebSocket messages.
        private void _webSocketClient_TextReceived(WebSocketClientTextReceivedEventArgs obj)
        {
            // Log the message content after removing curly braces for cleaner output.
            Print(obj.Text.Replace("{", "").Replace("}", "").ToString());
        }

        // This method is triggered on every tick; can handle price updates or related tasks.
        protected override void OnTick()
        {
            // Handle price updates here.
        }

        // This method is executed when the cBot stops.
        protected override void OnStop()
        {
            // Close the WebSocket connection with a normal closure status.
            _webSocketClient.Close(WebSocketClientCloseStatus.NormalClosure);
        }
    }
}
