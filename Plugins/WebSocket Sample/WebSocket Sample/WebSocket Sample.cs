// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//    
//    This sample uses the WebSocketClient to connect to a fictional API. When a piece of news is
//    released and the API exposes its contents, the sample updates the text displayed in a TextBlock
//    that is a child of an AspBlock.
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

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class WebSocketSample : Plugin
    {
        // Declaring our TextBlock that will display the news contents
        private TextBlock _textBlock = new TextBlock
        {
            Text = "Starting...",
            FontSize = 20,
            FontWeight = FontWeight.ExtraBold,
            TextAlignment = TextAlignment.Center,
            Padding = new Thickness(5, 5, 5, 5),
        };
        
        // _webSocketClientOptions allow us to define several settings
        // such as the keep-alive interval of the WebSocket connection
        private static WebSocketClientOptions _webSocketClientOptions = new WebSocketClientOptions 
        {
            KeepAliveInterval = new TimeSpan(0, 1, 30),
            UseDefaultCredentials = true,
        };
        
        // Passing our _webSocketClientOptions to the WebSocketClient
        // constructor
        private WebSocketClient _webSocketClient = new WebSocketClient(_webSocketClientOptions);
        
        // This API is entirely fictional
        private readonly Uri _targetUri = new Uri("ws://amazingnews.com:8000");
        
        protected override void OnStart()
        {
            // Connecting to the API and sending the initial message
            _webSocketClient.Connect(_targetUri);
            _webSocketClient.Send("Hello");
            
            // Declaring a custom handler for the TextReceived event
            _webSocketClient.TextReceived += NewsReceived;
            
            // Adding our TextBlock as a child of a custom
            // AspBlock
            var aspBlock = Asp.SymbolTab.AddBlock("News");
            aspBlock.IsExpanded = true;
            aspBlock.Height = 300;

            aspBlock.Child = _textBlock;
        }

        protected override void OnStop()
        {
            // The WebSocketClient must be disposed of in OnStop,
            // otherwise it will consume system resources
            _webSocketClient.Close(WebSocketClientCloseStatus.NormalClosure);
        }
        
        private void NewsReceived(WebSocketClientTextReceivedEventArgs args) 
        {
            // Updading the text inside the TextBlock on every
            // piece of news received
            if (args.Text.Length != 0) 
            {
                _textBlock.Text = args.Text;
            }
        }
    }        
}