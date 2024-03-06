// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses the WebSocketClient to connect to a fictional generative AI service. The sample
//    sends the initial prompt and, when a bar closes, it also sends a request to the AI to conduct
//    market analysis. On every response from the AI, the sample displays a message box containing
//    the generated text.
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
    public class WebSocketSample : Robot
    {
        // Creating the initial prompt for the AI
        private const string _initialPrompt = "You are an expert forex trader and market analyst";
        
        // Defining the necessary WebSocketClientOptions
        private static WebSocketClientOptions _webSocketClientOptions = new WebSocketClientOptions 
        {
            KeepAliveInterval = new TimeSpan(0, 1, 30),
            UseDefaultCredentials = true,
        };
        
        // Initialising the WebSocketClient
        private WebSocketClient _webSocketClient = new WebSocketClient(_webSocketClientOptions);
        
        // Defining the target URI where the AI service is available
        private readonly Uri _targetUri = new Uri("ws://chatddt.com:8000");
 
        protected override void OnStart()
        {
            // Connecting the WebSocketClient to the AI service
            // and handling events where a text response is received
            _webSocketClient.Connect(_targetUri);
            _webSocketClient.Send(_initialPrompt);
            _webSocketClient.TextReceived += webSocketClient_TextReceived;
        }

        protected override void OnBarClosed()
        {
            // Attaining data for the current bar that has just closed
            // and the preceding bar
            var currentBar = Bars.LastBar;
            var previousBar = Bars.Last(Bars.Count - 2);
            
            // Creating a prompt for market analysis based on bar data
            string marketPrompt = @$"
            For the current bar, the high, low, open, and close were the following:
            {currentBar.High}, {currentBar.Low}, {currentBar.Open}, {currentBar.Close}. For the previous bar,
            these values were {previousBar.High}, {previousBar.Low}, {previousBar.Open}, {previousBar.Close}.
            Make predictions about the future.
            ";
            
            // Sending the new prompt to the AI service
            _webSocketClient.Send(marketPrompt);
        }

        protected void webSocketClient_TextReceived(WebSocketClientTextReceivedEventArgs obj)
        {
            // Showing a MessageBox containing the text response
            // from the AI service
            MessageBox.Show(obj.Text);
        }

        protected override void OnStop()
        {
            // Disposing of the WebSocketClient to avoid memory leaks
            _webSocketClient.Close(WebSocketClientCloseStatus.NormalClosure);
        }
    }
}