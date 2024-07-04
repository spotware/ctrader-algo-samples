// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Plugins
{
  [Plugin(AccessRights = AccessRights.None)]
  public class InteractiveWebView : Plugin
  {
    WebView webView;

    protected override void OnStart()
    {
      webView = new WebView();
      webView.WebMessageReceived += webView_WebMessageReceived;

      var block = Asp.SymbolTab.AddBlock("Interactive WebView");
      block.Index = 1;
      block.Child = webView;
      block.Height = 100;
      block.IsExpanded = true;
      block.IsDetachable = false;

      webView.NavigationCompleted += webView_NavigationCompleted;
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


      Account.Switched += Account_Switched;
    }

    private void webView_NavigationCompleted(WebViewNavigationCompletedEventArgs obj)
    {
      UpdateAccountInfo();
    }

    private void Account_Switched(AccountSwitchedEventArgs obj)
    {
      UpdateAccountInfo();
    }

    private void webView_WebMessageReceived(WebViewWebMessageReceivedEventArgs args)
    {
      if (args.Message == @"""close all message""")
      {
        foreach (var position in Positions)
        {
          position.Close();
        }
      }
    }

    private void UpdateAccountInfo()
    {
      var data = new
      {
        ctid = Account.UserId,
        account = Account.Number,
        broker = Account.BrokerName,
      };
      var dataJson = System.Text.Json.JsonSerializer.Serialize(data);

      webView.ExecuteScript("updateValues(" + dataJson + ")");
    }
  }
}