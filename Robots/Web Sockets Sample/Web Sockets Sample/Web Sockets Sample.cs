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
    [Robot(AccessRights = AccessRights.None, AddIndicators = true)]
    public class WebSocketsExample : Robot
    {
        private WebSocketClient _webSocketClient = new WebSocketClient();
        private readonly Uri _targetUri = new Uri("wss://marketdata.tradermade.com/feedadv");

        protected override void OnStart()
        {
            _webSocketClient.Connect(_targetUri);

            _webSocketClient.TextReceived += _webSocketClient_TextReceived;

            var data = "{\"userKey\":\"PasteStreamingKeyHere\", \"symbol\":\"EURUSD\"}";

            _webSocketClient.Send(data);
        }

        private void _webSocketClient_TextReceived(WebSocketClientTextReceivedEventArgs obj)
        {
            Print(obj.Text.Replace("{", "").Replace("}", "").ToString());
        }

        protected override void OnTick()
        {
            // Handle price updates here
        }

        protected override void OnStop()
        {
            _webSocketClient.Close(WebSocketClientCloseStatus.NormalClosure);
        }
    }
}