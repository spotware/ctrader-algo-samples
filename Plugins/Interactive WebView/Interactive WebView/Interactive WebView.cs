// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
//    The sample adds a custom WebView to the Symbol tab, displaying account information
//    such as cTID, account number and broker name. It also includes a button that, when clicked, 
//    will close all open positions on the account.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
  // Declare the class as a plugin without requiring special access permissions.
  [Plugin(AccessRights = AccessRights.None)]
  public class InteractiveWebView : Plugin
  {
    WebView webView;  // Declare a WebView control to display custom HTML content.

    // This method is executed when the plugin starts.    
    protected override void OnStart()
    {
      webView = new WebView();  // Initialise the WebView control.
      webView.WebMessageReceived += webView_WebMessageReceived;  // Attach the event handler for receiving messages from the web content.

      // Create a new block on the Symbol tab to hold the WebView.
      var block = Asp.SymbolTab.AddBlock("Interactive WebView");
      block.Index = 1;
      block.Child = webView;
      block.Height = 100;
      block.IsExpanded = true;
      block.IsDetachable = false;

      // Attach the event handler for when the WebView has completed navigation.
      webView.NavigationCompleted += webView_NavigationCompleted;

      // Load custom HTML into the WebView, including the account info and 'Close all positions' button.
      webView.NavigateToStringAsync(@"
<body bgcolor='white'>
<div>
  <div>
    <span>ctid:</span>
    <span id='ctid'/>
  </div>
  <div>
    <span>account number:</span>
    <span id='account'/>
  </div>
  <div>
    <span>broker:</span>
    <span id='broker'/>
  </div>  
  <button onclick='closeAll()'>Close all positions</button>
<div>
</body>
<script>
function closeAll(){
  window.postMessage('close all message');
}
function updateValues(data){
  document.getElementById('ctid').innerHTML = data.ctid;
  document.getElementById('account').innerHTML = data.account;
  document.getElementById('broker').innerHTML = data.broker;
}
</script>
            ");

      // Attach event handler for when the account is switched.
      Account.Switched += Account_Switched;
    }

    // Event handler for when the WebView finishes loading the content.
    private void webView_NavigationCompleted(WebViewNavigationCompletedEventArgs obj)
    {
      UpdateAccountInfo();  // Update the account info once navigation is complete.
    }

    // Event handler for when the account is switched.
    private void Account_Switched(AccountSwitchedEventArgs obj)
    {
      UpdateAccountInfo();  // Update the account info whenever the account is switched.
    }

    // Event handler for receiving messages from the WebView content.
    private void webView_WebMessageReceived(WebViewWebMessageReceivedEventArgs args)
    {
      // Check if the message is the 'close all' message.
      if (args.Message == @"""close all message""")
      {
        foreach (var position in Positions)
        {
          position.Close();  // Close all open positions.
        }
      }
    }

    // Method to update the account info in the WebView.
    private void UpdateAccountInfo()
    {
      // Create an object to store account information.
      var data = new
      {
        ctid = Account.UserId,  // cTrader user ID.
        account = Account.Number,  // Account number.
        broker = Account.BrokerName,  // Broker name.
      };
      
      // Serialise the account info to JSON.
      var dataJson = System.Text.Json.JsonSerializer.Serialize(data);

      // Execute the JavaScript function in the WebView to update the values.
      webView.ExecuteScript("updateValues(" + dataJson + ")");
    }
  }
}
