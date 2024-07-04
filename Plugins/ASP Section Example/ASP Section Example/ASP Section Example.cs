// -------------------------------------------------------------------------------------------------
//
//    This code is a cTrader Algo API example.
//
//    This code is intended to be used as a sample and does not guarantee any particular outcome or
//    profit of any kind. Use it at your own risk.
//
// -------------------------------------------------------------------------------------------------

using cAlgo.API;

namespace cAlgo.Plugins
{
    [Plugin(AccessRights = AccessRights.None)]
    public class ASPSectionExample : Plugin
    {
        protected override void OnStart()
        {
            var block = Asp.SymbolTab.AddBlock("My title");
            block.Index = 2;
            block.Height = 500;
            block.IsExpanded = true;

            var webView = new WebView();
            block.Child = webView;

            webView.NavigateAsync("https://ctrader.com/");
        }
    }
}